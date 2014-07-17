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
                this.DataContext = (Contact)PhoneApplicationService.Current.State["SelectedContact"];
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
            var selectContact = this.DataContext as Contact;
            NativeFunctions.CallNumber(selectContact.Phone, selectContact.FullName);
        }

        /// <summary>
        /// Called when click to Add Contact button.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            var selectContact = this.DataContext as Contact;
            NativeFunctions.AddContactPeopleHub(selectContact);
        }
    }
}