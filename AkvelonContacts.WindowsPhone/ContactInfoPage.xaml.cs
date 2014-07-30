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
        /// Message if contact already exists
        /// </summary>
        private const string MessageIfContactAlreadyExists = "Сontact with the same name exists. Please rename the contact to avoid an unexpected result.";

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

                if (string.IsNullOrEmpty(this.contact.Phone))
                {
                    callButton.Visibility = Visibility.Collapsed;
                    sendSmsButton.Visibility = Visibility.Collapsed;
                }

                if (string.IsNullOrEmpty(this.contact.Mail))
                {
                    sendEmailButton.Visibility = Visibility.Collapsed;
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
            Communication.CallContact(this.contact);
        }

        /// <summary>
        /// Called when click to Add Contact button.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            Communication.ContactExists(
                this.contact,
                (bool isExists) =>
                {
                    if (!isExists)
                    {
                        Communication.AddContactToPeopleHub(this.contact);
                        return;
                    }

                    MessageBox.Show(MessageIfContactAlreadyExists, "Warning", MessageBoxButton.OK);
                    Communication.AddContactToPeopleHub(this.contact);
                });
        }

        /// <summary>
        /// Shows the Messaging application.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SendSMSButton_Click(object sender, RoutedEventArgs e)
        {
            Communication.SendSMSToContact(this.contact);
        }

        /// <summary>
        /// Shows the email application with a new message displayed.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            Communication.SendEmailToContact(this.contact);
        }
    }
}