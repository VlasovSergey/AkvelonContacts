//-----------------------------------------------------------------------
// <copyright file="DownloadComplitedEventArgs.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Contains DownloadCompleted event data.
    /// </summary>
    public class DownloadComplitedEventArgs : EventArgs
    {
        /// <summary>
        /// Contacts list.
        /// </summary>
        private List<Contact> result;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadComplitedEventArgs"/> class.
        /// </summary>
        /// <param name="contactList">Contacts list.</param>
        public DownloadComplitedEventArgs(List<Contact> contactList) : base()
        {
            this.result = contactList;
        }

        /// <summary>
        /// Gets the contacts list.
        /// </summary>
        public List<Contact> Result
        {
            get
            {
                return this.result;
            }
        }
    }
}
