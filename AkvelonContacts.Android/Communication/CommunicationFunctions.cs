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

namespace AkvelonContacts.Android
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
        public static void CallToContact(Contact contact, Context context)
        {
            // On "Call" button click, try to dial phone number.
            var callDialog = new AlertDialog.Builder(context);
            callDialog.SetMessage("Dial " + contact.FullName +" at " +contact.Phone + "?");
            callDialog.SetNeutralButton("Call", delegate
            {
                // Create intent to dial phone
                var callIntent = new Intent(Intent.ActionCall);
                callIntent.SetData(global::Android.Net.Uri.Parse("tel:" + contact.Phone));
                context.StartActivity(callIntent);
            });

            callDialog.SetNegativeButton("Don't call", delegate { });

            // Show the alert dialog to the user and wait for response.
            callDialog.Show();
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