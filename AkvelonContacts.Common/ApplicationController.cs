//-----------------------------------------------------------------------
// <copyright file="ApplicationController.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Downloads the contacts list.
    /// </summary>
    public class ApplicationController
    {
        /// <summary>
        /// Name for save to local storage.
        /// </summary>
        private const string JsonLocalName = "ContactList.json";

        /// <summary>
        /// Name of the directory to store the data of the application.
        /// </summary>
        private const string AppDataDirectoryName = "AppData\\";

        /// <summary>
        /// Name of the directory to store the image of the application.
        /// </summary>
        private const string ImagesDirectoryName = "images\\";

        /// <summary>
        /// Default images Extensions.
        /// </summary>
        private const string DefaultImageExtensions = ".jpeg";

        /// <summary>
        /// Url for photos download.
        /// </summary>
        private string photosStoreUrl = "http://prism.akvelon.net/api/system/getphoto/";

        /// <summary>
        /// URL for download contacts list.
        /// </summary>
        private string contactListJsonUrl = "http://prism.akvelon.net/api/employees/all";

        /// <summary>
        /// Gets or sets the Url for download contact list.
        /// </summary>
        private string ContactListJsonUrl
        {
            get
            {
                return this.contactListJsonUrl;
            }

            set
            {
                this.contactListJsonUrl = value;
            }
        }

        /// <summary>
        /// Gets photo by client id.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Photo physical path.</returns>
        public static string GetPhotoPathByClientId(string id)
        {
            return StorageController.GetPhysicalPathForLocalFilePath(GetImagePathById(id));
        }

        /// <summary>
        /// Loads contact list.
        /// </summary>
        /// <param name="action">Action when contact list is loaded without Photo.</param>
        /// <param name="onLoadPhoto">Action is called every time any photo loaded.</param>
        public void GetContacts(Action<List<Contact>> action, Action<Contact> onLoadPhoto)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.DownloadContactList(
                    (List<Contact> result) =>
                    {
                        List<Contact> contactList = result == null ? this.LoadLocalContactList() : result;
                        this.LoadPhotos(result, onLoadPhoto);
                        action(result);
                    });
            }
            else
            {
                var contacts = this.LoadLocalContactList();
                action(contacts);
                /* 
                this.LoadPhotos(contacts, onLoadPhoto);
                */
            }
        }

        /// <summary>
        /// Gets last update list time.
        /// </summary>
        /// <returns>Last update list time.</returns>
        public TimeSpan GetLastUpdateListTime() 
        {
            var lastWriteTime = StorageController.GetLastWriteTime(GetPathContactListJson());
            return TimeSpan.FromTicks(DateTimeOffset.Now.Ticks - lastWriteTime.Ticks);
        }

        /// <summary>
        /// Gets path for image by id.
        /// </summary>
        /// <param name="id">Id for image path generate.</param>
        /// <returns>Path for directory for images.</returns>
        private static string GetImagePathById(string id)
        {
            return GetDirrectoryNameforImages() + id + DefaultImageExtensions;
        }

        /// <summary>
        /// Gets path for directory for images.
        /// </summary>
        /// <returns>Path for directory for images.</returns>
        private static string GetDirrectoryNameforImages()
        {
            return AppDataDirectoryName + ImagesDirectoryName;
        }

        /// <summary>
        /// Gets path for directory for JSON file with contact list.
        /// </summary>
        /// <returns>Path for directory for JSON file with contact list.</returns>
        private static string GetPathContactListJson()
        {
            return AppDataDirectoryName + JsonLocalName;
        }

        /// <summary>
        /// Gets Contacts list.
        /// </summary>
        /// <param name="action">Action when the result came.</param>
        private void DownloadContactList(Action<List<Contact>> action)
        {
            FileDownloader.DownloadFileAsStringAsync(
                this.contactListJsonUrl,
                (string result) =>
                {
                    if (result == null)
                    {
                        action(null);
                        return;
                    }

                    action((new ContactsJsonParser()).GetListFromJsonArray(result));

                    if (!StorageController.DirectoryExists(AppDataDirectoryName))
                    {
                        StorageController.CreateDirectory(AppDataDirectoryName);
                    }

                    StorageController.WriteStringToFile(GetPathContactListJson(), result);
                });
        }

        /// <summary>
        /// Loads contacts list from local storage.
        /// </summary>
        /// <returns>Contacts list.</returns>
        private List<Contact> LoadLocalContactList()
        {
            if (!StorageController.FileExists(GetPathContactListJson()))
            {
                return new List<Contact>();
            }

            var json = StorageController.ReadAsString(GetPathContactListJson());
            return (new ContactsJsonParser()).GetListFromJsonArray(json);
        }

        /// <summary>
        /// Loads all photos for ContactList.
        /// </summary>
        /// <param name="contactList">Contact list.</param>
        /// <param name="onLoadPhoto">Action is called every time any photo loaded. Returns the contact which has been downloaded photo.</param>
        private void LoadPhotos(List<Contact> contactList, Action<Contact> onLoadPhoto)
        {
            foreach (var contact in contactList)
            {
                var photoPath = GetImagePathById(contact.Id);
                /*
                if (StorageController.FileExists(photoPath))
                {
                    onLoadPhoto(contact);
                    return;
                }
                */
                var contactPhotoUrl = this.photosStoreUrl + contact.Id;
                FileDownloader.DownloadFileAsync(
                    contactPhotoUrl,
                    (stream) =>
                    {
                        if (!StorageController.DirectoryExists(GetDirrectoryNameforImages()))
                        {
                            StorageController.CreateDirectory(GetDirrectoryNameforImages());
                        }

                        StorageController.CopyStreamToLocalStore(photoPath, stream);
                        var c = contact;
                        onLoadPhoto(contact);
                    });
            }
        }
    }
}
