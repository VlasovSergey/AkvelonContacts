//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AkvelonContacts.Common;
using AkvelonContacts.WindowsPhone.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Globalization;
using Microsoft.Phone.Shell;

namespace AkvelonContacts.WindowsPhone
{
    /// <summary>
    /// Main page.
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
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
            this.applicationCtrl = new ApplicationController();

            this.applicationCtrl.LoadContactList(
                (contactList) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        this.contactList = contactList;
                        DisplayContactList(this.contactList);
                    });
                },
                (contact) =>
                {
                    Dispatcher.BeginInvoke(() => { });
                });
        }

        /// <summary>
        /// Displays contact list.
        /// </summary>
        /// <param name="contactList">Contact list for display.</param>
        private void DisplayContactList(List<Contact> contactList)
        {
            List<AlphaKeyGroup<Contact>> dataSource = AlphaKeyGroup<Contact>.CreateGroups(
                contactList,
                new CultureInfo("ru-RU"),
                (Contact s) => { return s.FullName; },
                true);

            contactListSelector.ItemsSource = dataSource;
        }

        /// <summary>
        /// Called when selected changes.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Selection changed event args.</param>
        private void ContactListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (contactListSelector.SelectedItem != null)
            {
                VisualStateManager.GoToState(this.contactPanel, "ShowState", true);
                var selectedContact = (Contact)e.AddedItems[0];
                contactPanel.DataContext = selectedContact;

                contactPanel.Visibility = Visibility.Visible;
                contactListSelector.Visibility = Visibility.Collapsed;
                searchTextBox.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Called when selected changes.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (contactPanel.Visibility == Visibility.Visible)
            {
                contactPanel.Visibility = Visibility.Collapsed;
                contactListSelector.Visibility = Visibility.Visible;
                searchTextBox.Visibility = Visibility.Visible;
                contactListSelector.SelectedItem = null;
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Called when click to Call button.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            var selectContact = contactListSelector.SelectedItem as Contact;
            NativeFunctions.CallNumber(selectContact.Phone, selectContact.FullName);
        }

        /// <summary>
        /// Called when click to Add Contact button.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            var selectContact = contactListSelector.SelectedItem as Contact;
            NativeFunctions.AddContactPeopleHub(selectContact);
        }

        /// <summary>
        /// Called when change search field.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FocusManager.GetFocusedElement() != null && FocusManager.GetFocusedElement().Equals(this.searchTextBox))
            {
                var newList = this.contactList.Where(
                    item => item.FullName.IndexOf(searchTextBox.Text, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList<Contact>();
                this.DisplayContactList(newList);
            }
        }

        /// <summary>
        /// The foreground color of the text in searchTextBox is set to Magenta when searchTextBox.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == "Search")
            {
                this.searchTextBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// he foreground color of the text in searchTextBox is set to Blue when searchTextBox
        /// loses focus. Also, if SearchTB loses focus and no text is entered, the
        /// text "Search" is displayed.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == string.Empty)
            {
                searchTextBox.Text = "Search";
            }
        }

        /// <summary>
        /// Helper class for Grouping.
        /// </summary>
        /// <typeparam name="T">Type for grouping.</typeparam>
        private class AlphaKeyGroup<T> : List<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AlphaKeyGroup{T}" /> class.
            /// </summary>
            /// <param name="key">The key for this group.</param>
            public AlphaKeyGroup(string key)
            {
                this.Key = key;
            }

            /// <summary>
            /// The delegate that is used to get the key information.
            /// </summary>
            /// <param name="item">An object of type <typeparamref name="T"/></param>
            /// <returns>The key value to use for this object</returns>
            public delegate string GetKeyDelegate(T item);

            /// <summary>
            /// Gets the Key of this group.
            /// </summary>
            public string Key
            {
                get;
                private set;
            }

            /// <summary>
            /// Create a list of AlphaGroup <typeparamref name="T"/> with keys set by a SortedLocaleGrouping.
            /// </summary>
            /// <param name="items">The items to place in the groups.</param>
            /// <param name="ci">The CultureInfo to group and sort by.</param>
            /// <param name="getKey">A delegate to get the key from an item.</param>
            /// <param name="sort">Will sort the data if true.</param>
            /// <returns>An items source for a LongListSelector</returns>
            public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, GetKeyDelegate getKey, bool sort)
            {
                SortedLocaleGrouping slg = new SortedLocaleGrouping(ci);
                List<AlphaKeyGroup<T>> list = CreateGroups(slg);

                foreach (T item in items)
                {
                    int index = 0;

                    index = slg.GetGroupIndex(getKey(item));

                    if (index >= 0 && index < list.Count)
                    {
                        list[index].Add(item);
                    }
                }

                return list;
            }

            /// <summary>
            /// Create a list of AlphaGroup <typeparamref name="T"/> with keys set by a SortedLocaleGrouping.
            /// </summary>
            /// <param name="slg">The sorted locale grouping.</param>
            /// <returns>The items source for a LongListSelector.</returns>
            private static List<AlphaKeyGroup<T>> CreateGroups(SortedLocaleGrouping slg)
            {
                List<AlphaKeyGroup<T>> list = new List<AlphaKeyGroup<T>>();

                foreach (string key in slg.GroupDisplayNames)
                {
                    list.Add(new AlphaKeyGroup<T>(key));
                }

                return list;
            }
        }
    }
}