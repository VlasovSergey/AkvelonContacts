//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AkvelonContacts.Common;
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
        /// Url for contact info page.
        /// </summary>
        private const string ContactInfoPageUrl = "/ContactInfoPage.xaml";

        /// <summary>
        /// Background search text.
        /// </summary>
        private const string BackgroundSearchText = "Search";

        /// <summary>
        /// Application controller.
        /// </summary>
        private ApplicationController applicationCtrl;

        /// <summary>
        /// Value indicating whether display contacts only with key.
        /// </summary>
        private bool displayOnlyContactsWithKey;

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

            this.displayOnlyContactsWithKey = false;

            contactListSelector.ItemsSource = null;
            this.applicationCtrl = new ApplicationController();

            this.LoadContactsAndDisplay();
        }

        /// <summary>
        /// Called when a page becomes the active page in a frame.
        /// </summary>
        /// <param name="e">Cancel event args.</param>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            contactListSelector.SelectedItem = null;
            this.DisplayTimeUpdate();
        }

        /// <summary>
        /// Changes state display only contacts with key.
        /// </summary>
        /// <param name="appBarKeyButton">Application bar button</param>
        private void СhangesStateKeyOnly(ApplicationBarMenuItem appBarKeyButton)
        {
            if (this.displayOnlyContactsWithKey)
            {
                appBarKeyButton.Text = "show contacts with keys";
            }
            else
            {
                appBarKeyButton.Text = "show all contacts";
            }

            this.displayOnlyContactsWithKey = !this.displayOnlyContactsWithKey;
            this.DisplayContactList(this.contactList, null);
        }

        /// <summary>
        /// Shows progress indicator.
        /// </summary>
        private void ShowProgressIndicator()
        {
            progressBar.IsIndeterminate = true;
            progressBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides progress indicator.
        /// </summary>
        private void HideProgressIndicator()
        {
            progressBar.IsIndeterminate = false;
            progressBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Loads contacts.
        /// </summary>
        private void LoadContactsAndDisplay()
        {
            this.applicationCtrl.GetContacts(this.OnLoadContactList, this.OnLoadPhoto);
        }

        /// <summary>
        /// Download and display contacts.
        /// </summary>
        private void UpdateContactsFromServer()
        {
            this.applicationCtrl.DownloadContactsAndPhotos(this.OnLoadContactList, this.OnLoadPhoto);
        }

        /// <summary>
        /// Called when contact list is loaded without Photo.
        /// </summary>
        /// <param name="contactList">Contact list.</param>
        private void OnLoadContactList(List<Contact> contactList)
        {
            Dispatcher.BeginInvoke(() =>
            {
                this.HideProgressIndicator();
                if (contactList != null)
                {
                    this.contactList = contactList;
                    DisplayContactList(this.contactList, null);
                    this.DisplayTimeUpdate();
                }
                else
                {
                    this.HideProgressIndicator();
                    MessageBox.Show("Could not load contacts. Please check your internet connection.", "Warning", MessageBoxButton.OK);
                }
            });
        }

        /// <summary>
        /// Called every time any photo loaded.
        /// </summary>
        /// <param name="c">Contact which downloaded photo.</param>
        private void OnLoadPhoto(Contact c)
        {
        }

        /// <summary>
        /// Displays the time of last update.
        /// </summary>
        private void DisplayTimeUpdate()
        {
            DateTimeOffset? updateTimeOrNull = this.applicationCtrl.GetTimeOfLastUpdate();

            if (updateTimeOrNull == null)
            {
                return;
            }

            updateTimeTextBlock.Text = TimeController.GetElapsedTime((DateTimeOffset)updateTimeOrNull);
        }

        /// <summary>
        /// Displays contact list.
        /// </summary>
        /// <param name="contactList">Contact list for display.</param>
        /// <param name="contactFilter">Filter for contacts.</param>
        private void DisplayContactList(List<Contact> contactList, Func<Contact, bool> contactFilter)
        {
            var newList = ContactsFilter.FilterContacts(contactList, this.displayOnlyContactsWithKey, contactFilter);

            List<AlphaKeyGroup<Contact>> dataSource = AlphaKeyGroup<Contact>.CreateGroups(
                newList,
                new CultureInfo("ru-RU"),   // The culture to group contact list.
                (Contact s) => { return s.FullName; },
                true);

            contactListSelector.ItemsSource = dataSource;
        }

        /// <summary>
        /// Navigates on contactInfoPage.
        /// </summary>
        /// <param name="c">Contact for page.</param>
        private void NavigationOnContactInfoPage(Contact c)
        {
            var selectedContact = c;

            if (selectedContact == null)
            {
                return;
            }

            PhoneApplicationService.Current.State["SelectedContact"] = selectedContact;
            NavigationService.Navigate(new Uri(ContactInfoPageUrl, UriKind.Relative));
        }

        /// <summary>
        /// Called when selected changes.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Selection changed event args.</param>
        private void ContactListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.NavigationOnContactInfoPage((Contact)this.contactListSelector.SelectedItem);
        }

        /// <summary>
        /// Finds and displays contacts by text.
        /// </summary>
        /// <param name="searchText">Text for searching.</param>
        private void FindAndDisplayContactByText(string searchText)
        {
            this.DisplayContactList(
                this.contactList,
                (Contact contact) =>
                {
                    bool mailCriterion = contact.Mail != null && contact.Mail.IndexOf(searchText, System.StringComparison.OrdinalIgnoreCase) >= 0;
                    bool fullNameCriterion = contact.FullName.IndexOf(searchText, System.StringComparison.OrdinalIgnoreCase) >= 0;
                    return mailCriterion || fullNameCriterion;
                });
        }

        /// <summary>
        /// Shows search panel with focus and hides application bar.
        /// </summary>
        private void ShowSearchTextBox()
        {
            this.ApplicationBar.IsVisible = false;

            if (searchTextBox.Visibility != Visibility.Collapsed)
            {
                return;
            }

            ShowSearchBox.Begin();
            searchTextBox.Visibility = Visibility.Visible;
            searchTextBox.Focus();
        }

        /// <summary>
        /// Hides search panel with focus and shows application bar.
        /// </summary>
        private void HideSearchTextBox()
        {
            if (searchTextBox.Visibility != Visibility.Visible)
            {
                return;
            }

            searchTextBox.Text = string.Empty;
            this.ApplicationBar.IsVisible = true;
            HideSearchBox.Begin();
        }

        /// <summary>
        /// Called when change search field.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var focusElement = FocusManager.GetFocusedElement();

            if (this.searchTextBox.Text != string.Empty && !(FocusManager.GetFocusedElement() != null && FocusManager.GetFocusedElement().Equals(this.searchTextBox)))
            {
                return;
            }

            this.FindAndDisplayContactByText(this.searchTextBox.Text);
        }

        /// <summary>
        /// Removes text "Search" when searchTextBox gets focus.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == BackgroundSearchText)
            {
                this.searchTextBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// If SearchTB loses focus and no text is entered, the
        /// text "Search" is displayed.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchTextBox.Text == string.Empty)
            {
                searchTextBox.Text = BackgroundSearchText;
            }
        }

        /// <summary>
        /// Called when the button is clicked, Kay from application bar.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void AppBarKeyButton_Click(object sender, EventArgs e)
        {
            var appBarKeyButton = (ApplicationBarMenuItem)sender;
            this.СhangesStateKeyOnly(appBarKeyButton);
        }

        /// <summary>
        /// Called when the button is clicked, search.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void Search_Click(object sender, EventArgs e)
        {
            if (this.contactList == null)
            {
                MessageBox.Show("Contact list is not available.", "Warning", MessageBoxButton.OK);
            }

            this.ShowSearchTextBox();
        }

        /// <summary>
        /// Called when the button is clicked, refresh.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void Refresh_Click(object sender, EventArgs e)
        {
            this.ShowProgressIndicator();
            this.UpdateContactsFromServer();
        }

        /// <summary>
        /// Called when back button press.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.searchTextBox.Visibility == Visibility.Visible)
            {
                this.HideSearchTextBox();
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Called when a touch element.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            ((TextBlock)((Grid)sender).FindName("itemNameTextBlock")).Foreground = new SolidColorBrush((App.Current.Resources["PhoneAccentBrush"] as SolidColorBrush).Color);
        }

        /// <summary>
        /// Called when the element is removed touch.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            ((TextBlock)((Grid)sender).FindName("itemNameTextBlock")).Foreground = new SolidColorBrush((App.Current.Resources["PhoneForegroundBrush"] as SolidColorBrush).Color);
        }
    }
}