//-----------------------------------------------------------------------
// <copyright file="Communication.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AkvelonContacts.Common;
using Microsoft.Phone.Tasks;
using UserData = Microsoft.Phone.UserData;

namespace AkvelonContacts.WindowsPhone
{
    /// <summary>
    /// Contains methods that use native functions for Windows Phone.
    /// </summary>
    public class Communication
    {
        /// <summary>
        /// Calls to a phone number.
        /// </summary>
        /// <param name="contact">Contact for call.</param>
        public static void CallContact(Contact contact)
        {
            var call = new PhoneCallTask();
            call.PhoneNumber = contact.Phone;
            call.DisplayName = contact.FullName;
            call.Show();
        }

        /// <summary>
        /// Adds contact to People Hub.
        /// </summary>
        /// <param name="contact">Contact for saving</param>
        public static void AddContactToPeopleHub(Contact contact)
        {
            var saveContactTask = new SaveContactTask();

            saveContactTask.FirstName = contact.FirstName;
            saveContactTask.Company = Contact.CompanyName;
            saveContactTask.LastName = contact.LastName;
            saveContactTask.MobilePhone = contact.Phone;
            saveContactTask.WorkEmail = contact.Mail;

            saveContactTask.Show();
        }

        /// <summary>
        /// Checks the existence of a contact in People Hub.
        /// </summary>
        /// <param name="contact">Contact for test.</param>
        /// <param name="action">Return result.</param>
        public static void ContactExists(Contact contact, Action<bool> action)
        {
            UserData.Contacts cons = new UserData.Contacts();

            cons.SearchCompleted +=
            (object sender, UserData.ContactsSearchEventArgs e) =>
            {
                action(e.Results.Count() > 0);
            };

            cons.SearchAsync(contact.FullName, UserData.FilterKind.DisplayName, null);
        }

        /// <summary>
        /// Shows the Messaging application. 
        /// </summary>
        /// <param name="contact">Contact to send SMS.</param>
        public static void SendSMSToContact(Contact contact)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = contact.Phone;
            smsComposeTask.Show();
        }

        /// <summary>
        /// Shows the email application with a new message displayed.
        /// </summary>
        /// <param name="contact">Contact to send email.</param>
        public static void SendEmailToContact(Contact contact)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.To = contact.Mail;

            emailComposeTask.Show();
        }
    }
}
