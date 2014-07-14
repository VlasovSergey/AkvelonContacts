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
        /// Url for photos download.s
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

                    action(JsonParser.Deserialize<List<Contact>>(result));
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
                return JsonParser.Deserialize<List<Contact>>(json);
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
        /// <param name="onLoadPhoto">Action is called every time any photo loaded.</param>
        public void LoadPhotos(List<Contact> contactList, Action<Contact> onLoadPhoto)
        {
            foreach (var contact in contactList)
            {
                var photoName = contact.Id + ".jpeg";
                if (File.Exists(photoName))
                {
                    contact.PhotoPath = photoName;
                    onLoadPhoto(contact);
                }
                else
                {
                    var contactPhotoUrl = this.photosStoreUrl + contact.Id;
                    FileDownloader.DownloadFileAsync(
                        contactPhotoUrl,
                        (stream) =>
                        {
                            var localPath = StorageController.WriteStream(photoName, stream);
                            var c = contact;
                            contact.PhotoPath = localPath;
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
