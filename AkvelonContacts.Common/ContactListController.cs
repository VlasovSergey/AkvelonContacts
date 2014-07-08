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
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Downloads the contacts list.
    /// </summary>
    public class ContactListController
    {
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
        /// Delegate for event <see cref="DownloadCompleted"/>
        /// </summary>
        /// <param name="sender">Sender events.</param>
        /// <param name="e">Event args for <see cref="DownloadCompleted"/> event.</param>
        public delegate void DownloadComplitedHandler(object sender, DownloadComplitedEventArgs e);

        /// <summary>
        /// Occurs when the download is complete list of contacts.
        /// </summary>
        public event DownloadComplitedHandler DownloadCompleted;

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
        /// Downloads the contacts list.
        /// </summary>
        public void DownloadContactListAsync()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    var resultString = e.Result;
                    
                    var contactList = (new ContactsJsonParser()).GetListFromJsonArray(resultString);

                    this.DownloadCompleted(sender, new DownloadComplitedEventArgs(contactList));
                }
            };
            webClient.Encoding = Encoding.UTF8;
            webClient.DownloadStringAsync(new Uri(this.url));
        }
    }
}
