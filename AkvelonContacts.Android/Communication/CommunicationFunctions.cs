//-----------------------------------------------------------------------
// <copyright file="CommunicationFunctions.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AkvelonContacts.Common;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AkvelonContacts.Android.Communication
{
    /// <summary>
    /// Contains methods that use native functions for Windows Phone.
    /// </summary>
    public class CommunicationFunctions
    {
        /// <summary>
        /// Calls to a phone number.
        /// </summary>
        /// <param name="contact">Contact for call.</param>
        public static void CallToContact(Contact contact)
        {
        }

        /// <summary>
        /// Adds contact to People Hub.
        /// </summary>
        /// <param name="contact">Contact for saving</param>
        public static void AddContactPeopleHub(Contact contact)
        {
        }

        /// <summary>
        /// Checks the existence of a contact in People Hub.
        /// </summary>
        /// <param name="contact">Contact for test.</param>
        /// <param name="action">Return result.</param>
        public static void ContactExists(Contact contact, Action<bool> action)
        {
        }

        /// <summary>
        /// Shows the Messaging application. 
        /// </summary>
        /// <param name="contact">Contact to send SMS.</param>
        public static void SendSMSToContact(Contact contact)
        {
        }

        /// <summary>
        /// Shows the email application with a new message displayed.
        /// </summary>
        /// <param name="contact">Contact to send email.</param>
        public static void SendEmailToContact(Contact contact)
        {
        }
    }
}