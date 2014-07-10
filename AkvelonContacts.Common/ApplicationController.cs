//-----------------------------------------------------------------------
// <copyright file="ApplicationController.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private const string JsonLocalName = "ContactsList.json";

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
        public void DownloadContactsList(Action<List<Contact>> action)
        {
            FileDownloader.DownloadStringAsync(
                this.url,
                Encoding.UTF8,
                (string result) =>
                {
                    if (result == null)
                    {
                        action(null);
                        return;
                    }

                    action((new ContactsJsonParser()).GetListFromJsonArray(result));
                    StorageController.WriteString(JsonLocalName, result);
                });
        }

        /// <summary>
        /// Loads contacts list from local storage.
        /// </summary>
        /// <returns>Contacts list.</returns>
        public List<Contact> LoadLocalContactsList()
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
        /// <param name="onLoadPhoto">Action is called every time any photo loaded.</param>
        public void LoadAllPhotosAsync(List<Contact> contactList, Action<Contact> onLoadPhoto)
        {
            foreach (var contact in contactList)
            {
                var photoName = contact.Id + ".jpeg";
                if (StorageController.FileExists(photoName))
                {
                    contact.PhotoPath = photoName;
                    onLoadPhoto(contact);
                }
                else
                {
                    var contactPhotoUrl = this.photosStoreUrl + contact.Id;
                    FileDownloader.DownloadStreamAsync(
                        contactPhotoUrl,
                        (stream) =>
                        {
                            StorageController.WriteStream(photoName, stream);
                            var c = contact;
                            contact.PhotoPath = photoName;
                            onLoadPhoto(contact);
                        });
                }
            }
        }
    }
}
