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
    public class NativeFunctions
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
            saveContactTask.Company = "Akvelon";
            saveContactTask.LastName = c.LastName;
            saveContactTask.MobilePhone = c.Phone;
            saveContactTask.WorkEmail = c.Mail;
            
            saveContactTask.Show();
        }
    }
}
