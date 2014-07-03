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
        /// Size of call button.
        /// </summary>
        private const double CallButtonSize = 115;

        /// <summary>
        /// Font size of name text.
        /// </summary>
        private const double NameTextFontSize = 35;

        /// <summary>
        /// Font size of phone text.
        /// </summary>
        private const double PhoneTextFontSize = 25;

        /// <summary>
        /// Indent of phone text.
        /// </summary>
        private const double PhoneTextIndent = 20;

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
            var jc = new ClientsListDownloader();
            jc.DownloadCompleted += (object sender, DownloadComplitedEventArgs e) =>
            {
                this.contactList = e.Result;
                this.DisplayContactList();
            };
            jc.DownloadContactListAsync(URL);
        }
        
        /// <summary>
        /// Displays the contacts list.
        /// </summary>
        private void DisplayContactList()
        {
            this.ContactList_sp.Children.Clear();

            foreach (var cont in this.contactList)
            {
                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition());
                var cd = new ColumnDefinition();

                cd.Width = new GridLength(CallButtonSize, GridUnitType.Pixel);
                grid.ColumnDefinitions.Add(cd);

                grid.RowDefinitions.Add(new RowDefinition());
                grid.RowDefinitions.Add(new RowDefinition());

                TextBlock tboxName = new TextBlock();
                tboxName.FontSize = NameTextFontSize;
                tboxName.Text = cont.FirstName + " " + cont.LastName;

                TextBlock tboxPhone = new TextBlock();
                tboxPhone.FontSize = PhoneTextFontSize;
                tboxPhone.Text = cont.Telephone;
                tboxPhone.Margin = new System.Windows.Thickness { Left = PhoneTextIndent };
                Grid.SetRow(tboxPhone, 1);

                Button btnCall = new Button();
                btnCall.Height = CallButtonSize;
                btnCall.Width = CallButtonSize;
                btnCall.Content = "Call";
                Grid.SetRowSpan(btnCall, 2);
                Grid.SetColumn(btnCall, 1);

                grid.Children.Add(tboxName);
                grid.Children.Add(tboxPhone);
                grid.Children.Add(btnCall);

                ContactList_sp.Children.Add(grid);
            }
        }
    }
}