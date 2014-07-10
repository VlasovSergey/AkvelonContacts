//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
        /// Application controller.
        /// </summary>
        private ApplicationController applicationCtrl;

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
            this.applicationCtrl = new ApplicationController(URL);

            this.LoadAndShowContactList();
            
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
        }
        
        /// <summary>
        /// Loads and shows contact list.
        /// </summary>
        private void LoadAndShowContactList() 
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                Dispatcher.BeginInvoke(() =>
                {
                    applicationCtrl.DownloadContactsList(
                        (List<Contact> result) =>
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                if (result == null)
                                {
                                    this.contactList = applicationCtrl.LoadLocalContactsList();
                                }
                                else
                                {
                                    this.contactList = result;
                                }

                                this.DisplayContactList();
                            });
                        });
                });
            }
            else
            {
                this.contactList = this.applicationCtrl.LoadLocalContactsList();
                this.DisplayContactList();
            }
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