//-----------------------------------------------------------------------
// <copyright file="MainPage.xaml.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AkvelonContacts.Common;
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
            progressBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Hides progress indicator.
        /// </summary>
        private void HideProgressIndicator()
        {
            progressBar.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Loads contacts.
        /// </summary>
        private void LoadContactsAndDisplay()
        {
            this.applicationCtrl.GetContacts(
            (contactList) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (contactList != null)
                    {
                        this.contactList = contactList;
                        DisplayContactList(this.contactList, null);
                        this.DisplayTimeUpdate();
                    }
                    else
                    {
                        this.HideProgressIndicator();
                        MessageBox.Show("Could not load contacts.", "Warning", MessageBoxButton.OK);
                    }
                });
            },
            (contact) =>
            {
                Dispatcher.BeginInvoke(() => { });
            });
        }

        /// <summary>
        /// Displays the time of last update.
        /// </summary>
        private void DisplayTimeUpdate()
        {
            string text = string.Empty;

            TimeSpan? updateTimeOrNull = this.applicationCtrl.GetLastUpdateListTime();

            if (updateTimeOrNull == null)
            {
                return;
            }

            TimeSpan updateTime = (TimeSpan)updateTimeOrNull;

            if (updateTime.Days != 0)
            {
                text += updateTime.Days + " Days ";
            }

            if (updateTime.Hours != 0)
            {
                text += updateTime.Hours + " Hours ";
            }

            if (updateTime.Minutes != 0)
            {
                text += updateTime.Minutes + " Min ";
            }

            if (updateTime.Seconds != 0)
            {
                text += updateTime.Seconds + " Sec ";
            }

            if (text != string.Empty)
            {
                text = "Updated " + text + "ago";
            }
            else
            {
                text = "Now updated";
            }

            updateTimeTextBlock.Text = text;
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
        /// Finds and displays contacts for text from textBox.
        /// </summary>
        /// <param name="tb">TextBox for searching.</param>
        private void FindAndDisplayContactByTextBox(TextBox tb)
        {
            var focusElement = FocusManager.GetFocusedElement();
            var searchText = tb.Text;

            if (searchText != string.Empty && !(FocusManager.GetFocusedElement() != null && FocusManager.GetFocusedElement().Equals(tb)))
            {
                return;
            }

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
        /// <returns>Is method executed.</returns>
        private bool ShowSearch()
        {
            if (this.contactList == null)
            {
                MessageBox.Show("Contacts are not loaded.", "Warning", MessageBoxButton.OK);
                return false;
            }

            this.ApplicationBar.IsVisible = false;

            if (searchTextBox.Visibility != Visibility.Collapsed)
            {
                return false;
            }

            ShowSearchBox.Begin();
            searchTextBox.Visibility = Visibility.Visible;
            searchTextBox.Focus();
            return true;
        }

        /// <summary>
        /// Hides search panel with focus and shows application bar.
        /// </summary>
        /// <returns>Is method executed.</returns>
        private bool HideSearch()
        {
            if (searchTextBox.Visibility != Visibility.Visible)
            {
                return false;
            }

            searchTextBox.Text = string.Empty;
            this.ApplicationBar.IsVisible = true;
            HideSearchBox.Begin();
            return true;
        }

        /// <summary>
        /// Called when change search field.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FindAndDisplayContactByTextBox(this.searchTextBox);
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
            this.ShowSearch();
        }

        /// <summary>
        /// Called when the button is clicked, refresh.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void Refresh_Click(object sender, EventArgs e)
        {
            this.LoadContactsAndDisplay();
        }

        /// <summary>
        /// Called when back button press.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.HideSearch())
            {
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
                
                if (sort)
                {
                    foreach (AlphaKeyGroup<T> group in list)
                    {
                        group.Sort((c0, c1) => { return ci.CompareInfo.Compare(getKey(c0), getKey(c1)); });
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