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
        /// First name of contact.
        /// </summary>
        private string firstName;

        /// <summary>
        /// Last name of contact.
        /// </summary>
        private string lastName;

        /// <summary>
        /// Description of contact.
        /// </summary>
        private string description;

        /// <summary>
        /// Id of contact.
        /// </summary>
        private string id;

        /// <summary>
        /// Mail of contact.
        /// </summary>
        private string mail;

        /// <summary>
        /// Skype of contact.
        /// </summary>
        private string skype;

        /// <summary>
        /// Phone number of contact.
        /// </summary>
        private string telephone;

        /// <summary>
        /// Gets or sets the first name of contact.
        /// </summary>
        public string FirstName
        {
            get 
            { 
                return this.firstName; 
            }

            set 
            {
                this.firstName = value; 
            }
        }

        /// <summary>
        /// Gets or sets the Last name of contact.
        /// </summary>
        public string LastName
        {
            get
            {
                return this.lastName;
            }

            set
            {
                this.lastName = value;
            }
        }

        /// <summary>
        /// Gets or sets the description of contact.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets the id of contact.
        /// </summary>
        public string Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Gets or sets the skype of contact.
        /// </summary>
        public string Skype
        {
            get
            {
                return this.skype;
            }

            set
            {
                this.skype = value;
            }
        }

        /// <summary>
        /// Gets or sets the mail of contact.
        /// </summary>
        public string Mail
        {
            get
            {
                return this.mail;
            }

            set
            {
                this.mail = value;
            }
        }

        /// <summary>
        /// Gets or sets the phone number of contact.
        /// </summary>
        public string Telephone
        {
            get
            {
                return this.telephone;
            }

            set
            {
                this.telephone = value;
            }
        }
    }
}
