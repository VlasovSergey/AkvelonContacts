//-----------------------------------------------------------------------
// <copyright file="ClientsListDownloader.cs" company="Akvelon">
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
    /// Downloads the clients list.
    /// </summary>
    public class ClientsListDownloader
    {
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
        /// Downloads the clients list.
        /// </summary>
        /// <param name="url">Url for download.</param>
        public void DownloadContactListAsync(string url)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    var resultString = e.Result;
                    this.DownloadCompleted(sender, new DownloadComplitedEventArgs(this.GetContactsListFromJson(resultString)));
                }
            };
            wc.Encoding = Encoding.UTF8;
            wc.DownloadStringAsync(new Uri(url));
        }

        /// <summary>
        /// Gets the contacts list from JSON string.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>Contacts list</returns>
        private List<Contact> GetContactsListFromJson(string json)
        {
            List<Contact> contList = new List<Contact>();
            var ja = JArray.Parse(json);

            foreach (JObject jo in ja)
            {
                Contact c = this.ConvertJObjectToContact(jo);
                contList.Add(c);
            }

            return contList;
        }

        /// <summary>
        /// Converts JObject to contact.
        /// </summary>
        /// <param name="jo">JObject with contact data.</param>
        /// <returns>Convert contact.</returns>
        private Contact ConvertJObjectToContact(JObject jo)
        {
            var c = new Contact();

            c.Description = (string)jo["Description"];
            c.FirstName = (string)jo["FirstName"];
            c.Id = (string)jo["Id"];
            c.LastName = (string)jo["LastName"];
            c.Mail = (string)jo["Mail"];
            c.Skype = (string)jo["Skype"];
            c.Telephone = (string)jo["Telephone"];

            return c;
        }
    }
}
