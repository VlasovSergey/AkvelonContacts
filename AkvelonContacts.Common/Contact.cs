//-----------------------------------------------------------------------
// <copyright file="Contact.cs" company="Akvelon">
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
    ///  Stores data for contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets the full name of contact.
        /// </summary>
        public string FullName
        {
            get
            {
                return this.FirstName + ' ' + this.LastName;
            }
        }

        /// <summary>
        /// Gets or sets the first name of contact.
        /// </summary>
        public string FirstName
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the Last name of contact.
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description of contact.
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the id of contact.
        /// </summary>
        public string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the skype of contact.
        /// </summary>
        public string Skype
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mail of contact.
        /// </summary>
        public string Mail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the phone number of contact.
        /// </summary>
        public string Telephone
        {
            get;
            set;
        }
    }
}
