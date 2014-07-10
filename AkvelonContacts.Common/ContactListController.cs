//-----------------------------------------------------------------------
// <copyright file="ContactListController.cs" company="Akvelon">
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
    public class ContactListController
    {
        /// <summary>
        /// Name for save to local storage.
        /// </summary>
        private const string JsonLocalName = "ContactsList.json";

        /// <summary>
        /// URL for download contacts list.
        /// </summary>
        private string url;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactListController"/> class.
        /// </summary>
        /// <param name="url">URL to JSON with customers</param>
        public ContactListController(string url)
        {
            this.url = url;
        }

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
    }
}
