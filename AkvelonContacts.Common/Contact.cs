﻿//-----------------------------------------------------------------------
// <copyright file="Contact.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace AkvelonContacts.Common
{
    /// <summary>
    /// Stores data for contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets company name.
        /// </summary>
        public static string CompanyName
        {
            get { return "Akvelon"; }
        }

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
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last name of contact.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the description of contact.
        /// </summary>
        public string Dislocation { get; set; }

        /// <summary>
        /// Gets or sets the id of contact.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the skype of contact.
        /// </summary>
        public string Skype { get; set; }

        /// <summary>
        /// Gets or sets the mail of contact.
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Gets or sets the phone number of contact.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contact has a key.
        /// </summary>
        public bool SecurityKey { get; set; }
    }
}
