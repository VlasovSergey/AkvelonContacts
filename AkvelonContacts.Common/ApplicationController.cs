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
        private const string JsonLocalName = "AppData\\ContactList.json";

        /// <summary>
        /// Name of the directory to store the image of the application.
        /// </summary>
        private const string AppDataDirectoryName = "AppData\\";

        /// <summary>
        /// Name of the directory to store the image of the application.
        /// </summary>
        private const string ImagesDirectoryName = "AppData\\images\\";

        /// <summary>
        /// Url for photos download.
        /// </summary>
        private string photosStoreUrl = "http://prism.akvelon.net/api/system/getphoto/";

        /// <summary>
        /// URL for download contacts list.
        /// </summary>
        private string url = "http://prism.akvelon.net/api/employees/all";

        /// <summary>
        /// Gets or sets the Url for download contact list.
        /// </summary>
        public string Url
        {
            get
            {
                return this.url;
            }

            set
            {
                this.url = value;
            }
        }

        /// <summary>
        /// Gets photo by client id.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Photo physical path.</returns>
        public static string GetPhotoPathByClientId(string id)
        {
            return StorageController.GetPhysicalPathForLocalFilePath(ImagesDirectoryName + id + ".jpeg");
        }

        /// <summary>
        /// Gets Contacts list.
        /// </summary>
        /// <param name="action">Action when the result came.</param>
        public void DownloadContactList(Action<List<Contact>> action)
        {
            FileDownloader.DownloadFileAsStringAsync(
                this.url,
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

                    StorageController.WriteString(JsonLocalName, result);
                });
        }

        /// <summary>
        /// Loads contacts list from local storage.
        /// </summary>
        /// <returns>Contacts list.</returns>
        public List<Contact> LoadLocalContactList()
        {
            if (StorageController.FileExists(JsonLocalName))
            {
                var json = StorageController.ReadString(JsonLocalName);
                return (new ContactsJsonParser()).GetListFromJsonArray(json);
            }
            else
            {
                return new List<Contact>();
            }
        }

        /// <summary>
        /// Loads all photos for ContactList.
        /// </summary>
        /// <param name="contactList">Contact list.</param>
        /// <param name="onLoadPhoto">Action is called every time any photo loaded. Returns the contact which has been downloaded photo.</param>
        public void LoadPhotos(List<Contact> contactList, Action<Contact> onLoadPhoto)
        {
            foreach (var contact in contactList)
            {
                var photoPath = ImagesDirectoryName + contact.Id + ".jpeg";
                if (StorageController.FileExists(photoPath))
                {
                    onLoadPhoto(contact);
                }
                else
                {
                    var contactPhotoUrl = this.photosStoreUrl + contact.Id;
                    FileDownloader.DownloadFileAsync(
                        contactPhotoUrl,
                        (stream) =>
                        {
                            if (!StorageController.DirectoryExists(ImagesDirectoryName))
                            {
                                StorageController.CreateDirectory(ImagesDirectoryName);
                            }

                            StorageController.WriteStream(photoPath, stream);
                            var c = contact;
                            onLoadPhoto(contact);
                        });
                }
            }
        }

        /// <summary>
        /// Loads contact list.
        /// </summary>
        /// <param name="action">Action when contact list is loaded without Photo.</param>
        /// <param name="onLoadPhoto">Action is called every time any photo loaded.</param>
        public void LoadContactList(Action<List<Contact>> action, Action<Contact> onLoadPhoto)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                this.DownloadContactList(
                    (List<Contact> result) =>
                    {
                        if (result == null)
                        {
                            action(this.LoadLocalContactList());
                        }
                        else
                        {
                            action(result);
                        }

                        this.LoadPhotos(result, onLoadPhoto);
                    });
            }
            else
            {
                var contacts = this.LoadLocalContactList();
                action(contacts);
                this.LoadPhotos(contacts, onLoadPhoto);
            }
        }
    }
}
