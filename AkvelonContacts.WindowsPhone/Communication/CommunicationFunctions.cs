//-----------------------------------------------------------------------
// <copyright file="NativeFunctions.cs" company="Akvelon">
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

namespace AkvelonContacts.WindowsPhone
{
    /// <summary>
    /// Contains methods that use native functions for Windows Phone.
    /// </summary>
    public class CommunicationFunctions
    {
        /// <summary>
        /// Calls to a phone number.
        /// </summary>
        /// <param name="phoneNumber">Phone number for call.</param>
        /// <param name="displayName">Name that is displayed when the Phone application is launched.</param>
        public static void CallNumber(string phoneNumber, string displayName)
        {
            var call = new PhoneCallTask();
            call.PhoneNumber = phoneNumber;
            call.DisplayName = displayName;
            call.Show();
        }

        /// <summary>
        /// Adds contact to People Hub.
        /// </summary>
        /// <param name="c">Contact for saving</param>
        public static void AddContactPeopleHub(Contact c)
        {
            var saveContactTask = new SaveContactTask();

            saveContactTask.FirstName = c.FirstName;
            saveContactTask.Company = Contact.CompanyName;
            saveContactTask.LastName = c.LastName;
            saveContactTask.MobilePhone = c.Phone;
            saveContactTask.WorkEmail = c.Mail;

            saveContactTask.Show();
        }

        /// <summary>
        /// Shows the Messaging application. 
        /// </summary>
        /// <param name="number">The recipient list for the new SMS message.</param>
        public static void SendSMS(string number)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = number;

            smsComposeTask.Show();
        }

        /// <summary>
        /// Shows the email application with a new message displayed.
        /// </summary>
        /// <param name="email">The recipients on the To line of the new email message.</param>
        public static void SendEmail(string email)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.To = email;

            emailComposeTask.Show();
        }
    }
}
