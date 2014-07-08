//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Akvelon">
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
using AkvelonContacts.WindowsPhone.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AkvelonContacts.WindowsPhone
{
    /// <summary>
    /// Main page.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// URL for download contacts list.
        /// </summary>
        private const string URL = "http://prism.akvelon.net/api/employees/all";

        /// <summary>
        /// Contains contact list.
        /// </summary>
        private List<Contact> contactList;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            var contactListCtrl = new ContactListController(URL);
            contactListCtrl.DownloadCompleted += (object sender, DownloadComplitedEventArgs e) =>
            {
                this.contactList = e.Result;
                this.DisplayContactList();
            };

            ContactsListBox.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                if (e.AddedItems != null)
                {
                    buttonCall.Visibility = buttonInfo.Visibility = Visibility.Visible;
                }
                else
                {
                    buttonCall.Visibility = buttonInfo.Visibility = Visibility.Collapsed;
                }
            };

            contactListCtrl.DownloadContactListAsync();
        }
        
        /// <summary>
        /// Displays the contacts list.
        /// </summary>
        private void DisplayContactList()
        {
            this.ContactsListBox.ItemsSource = this.contactList;
        }
    }
}