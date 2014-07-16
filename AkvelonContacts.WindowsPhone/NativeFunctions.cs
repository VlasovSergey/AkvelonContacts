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
    }
}
