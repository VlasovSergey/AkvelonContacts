﻿//-----------------------------------------------------------------------
// <copyright file="ApplicationController.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;

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
        private const string ContactsLocalFileName = "ContactList.json";

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
        private const string PphotosStorageUrl = "http://prism.akvelon.net/api/system/getphoto/";

        /// <summary>
        /// URL for download contacts list.
        /// </summary>
        private const string ContactListUrl = "http://prism.akvelon.net/api/employees/all";

        /// <summary>
        /// Gets stream for image file by contact id.
        /// </summary>
        /// <param name="id">Contact id.</param>
        /// <returns>Stream for image file.</returns>
        public static Stream GetImageStreamByContactId(string id)
        {
            return StorageController.GetStreamOfFileForRead(ApplicationController.GetImagePathByContactId(id));
        }

        /// <summary>
        /// Gets photo by client id.
        /// </summary>
        /// <param name="id">Client id.</param>
        /// <returns>Photo physical path.</returns>
        public static string GetPhysicalPathByContactId(string id)
        {
            return StorageController.GetPhysicalPathForLocalFilePath(GetImagePathByContactId(id));
        }

        /// <summary>
        /// Gets path for image by id.
        /// </summary>
        /// <param name="id">Id for image path generate.</param>
        /// <returns>Path for directory for images.</returns>
        public static string GetImagePathByContactId(string id)
        {
            return GetDirrectoryNameforImages() + id + DefaultImageExtensions;
        }

        /// <summary>
        /// Gets url for image by id.
        /// </summary>
        /// <param name="id">Id for image url generate.</param>
        /// <returns>Url for images.</returns>
        public static string GetImageUrlByContactId(string id)
        {
            return PphotosStorageUrl + id;
        }

        /// <summary>
        /// Loads contact list. Returns null if it can not load contacts.
        /// </summary>
        /// <param name="action">Action when contact list is loaded without Photo.</param>
        /// <param name="onLoadPhoto">Action is called every time any photo loaded.</param>
        public void GetContacts(Action<List<Contact>> action, Action<Contact> onLoadPhoto)
        {
            var localContacts = this.LoadLocalContactList();
            var networkAvailable = NetworkInterface.GetIsNetworkAvailable();

            if (localContacts != null)
            {
                action(localContacts);
            }

            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                if (localContacts == null)
                {
                    action(null);
                }

                return;
            }

            this.DownloadContactsAndPhotos(
                (result) =>
                {
                    if (result != null)
                    {
                        action(result);
                    }
                    else
                    {
                        if (localContacts == null)
                        {
                            action(null);
                        }
                    }
                },
                onLoadPhoto);
        }

        /// <summary>
        /// Downloads contactList and Photos. Returns null if it can not load contacts.
        /// </summary>
        /// <param name="action">Action when contact list is loaded without Photo.</param>
        /// <param name="onLoadPhoto">Action is called every time any photo loaded.</param>
        public void DownloadContactsAndPhotos(Action<List<Contact>> action, Action<Contact> onLoadPhoto)
        {
            this.DownloadContactList(
                (List<Contact> result) =>
                {
                    if (result != null)
                    {
                        this.LoadPhotos(result, onLoadPhoto);
                    }

                    action(result);
                });
        }

        /// <summary>
        /// Gets last update list time.
        /// </summary>
        /// <returns>Last update list time.</returns>
        public DateTimeOffset? GetTimeOfLastUpdate()
        {
            DateTimeOffset? lastWriteTimeOrNull = StorageController.GetLastWriteTime(GetPathContactListJson());

            return lastWriteTimeOrNull;
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
            return AppDataDirectoryName + ContactsLocalFileName;
        }

        /// <summary>
        /// Gets Contacts list.
        /// </summary>
        /// <param name="action">Action when the result came.</param>
        private void DownloadContactList(Action<List<Contact>> action)
        {
            FileDownloader.DownloadFileAsString(
                ContactListUrl,
                (string result) =>
                {
                    if (result == null)
                    {
                        action(null);
                        return;
                    }

                    var contactList = (new ContactsJsonParser()).GetListFromJsonArray(result);

                    action(contactList);

                    if (contactList == null)
                    {
                        return;
                    }

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
                return null;
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
            if (!StorageController.DirectoryExists(GetDirrectoryNameforImages()))
            {
                StorageController.CreateDirectory(GetDirrectoryNameforImages());
            }

            foreach (var contact in contactList)
            {
                var photoPath = GetImagePathByContactId(contact.Id);
                /*
                if (StorageController.FileExists(photoPath))
                {
                    onLoadPhoto(contact);
                    return;
                }
                */
                var contactPhotoUrl = GetImageUrlByContactId(contact.Id);
                FileDownloader.DownloadFile(
                    contactPhotoUrl,
                    (stream) =>
                    {
                        if (stream != null)
                        {
                            StorageController.CopyStreamToLocalStoreage(photoPath, stream);
                            onLoadPhoto(contact);
                        }
                    });
            }
        }
    }
}
