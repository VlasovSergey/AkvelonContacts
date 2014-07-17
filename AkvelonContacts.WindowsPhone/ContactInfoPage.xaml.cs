//-----------------------------------------------------------------------
// <copyright file="ContactInfoPage.xaml.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AkvelonContacts.Common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AkvelonContacts.WindowsPhone
{
    /// <summary>
    /// Contact info page.
    /// </summary>
    public partial class ContactInfoPage : PhoneApplicationPage
    {
        /// <summary>
        /// Selected contact.
        /// </summary>
        private Contact contact;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactInfoPage" /> class.
        /// </summary>
        public ContactInfoPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (PhoneApplicationService.Current.State.ContainsKey("SelectedContact"))
            {
                this.contact = (Contact)PhoneApplicationService.Current.State["SelectedContact"];
                this.DataContext = this.contact;

                if (this.contact.Phone == string.Empty || this.contact.Phone == null)
                {
                    callButton.Visibility = Visibility.Collapsed;
                }
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Called when click to Call button.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            NativeFunctions.CallNumber(this.contact.Phone, this.contact.FullName);
        }

        /// <summary>
        /// Called when click to Add Contact button.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            NativeFunctions.AddContactPeopleHub(this.contact);
        }

        /// <summary>
        /// Shows the Messaging application.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SendSMS_Click(object sender, RoutedEventArgs e)
        {
            NativeFunctions.SendSMS(this.contact.Phone);
        }

        /// <summary>
        /// Shows the email application with a new message displayed.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            NativeFunctions.SendEmail(this.contact.Mail);
        }
    }
}