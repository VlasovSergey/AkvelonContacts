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
using System.Windows.Navigation;
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

            this.LoadAndShowContactList();
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

                                this.OnLoadContactList();
                            });
                        });
                });
            }
            else
            {
                this.contactList = this.applicationCtrl.LoadLocalContactsList();
                this.OnLoadContactList();
            }
        }

        /// <summary>
        /// Displays the contacts list.
        /// </summary>
        private void OnLoadContactList()
        {
            List<AlphaKeyGroup<Contact>> dataSource = AlphaKeyGroup<Contact>.CreateGroups(
                this.contactList,
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                (Contact s) => { return s.FullName; },
                true);
            ContactsListBox.ItemsSource = dataSource;

            this.applicationCtrl.LoadAllPhotosAsync(this.contactList, (contact) => { });
        }

        /// <summary>
        /// Helper class for Grouping.
        /// </summary>
        /// <typeparam name="T">Type for grouping.</typeparam>
        public class AlphaKeyGroup<T> : List<T>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AlphaKeyGroup<T>" /> class.
            /// </summary>
            /// <param name="key">The key for this group.</param>
            public AlphaKeyGroup(string key)
            {
                this.Key = key;
            }

            /// <summary>
            /// The delegate that is used to get the key information.
            /// </summary>
            /// <param name="item">An object of type T</param>
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
            /// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping.
            /// </summary>
            /// <param name="items">The items to place in the groups.</param>
            /// <param name="ci">The CultureInfo to group and sort by.</param>
            /// <param name="getKey">A delegate to get the key from an item.</param>
            /// <param name="sort">Will sort the data if true.</param>
            /// <returns>An items source for a LongListSelector</returns>
            public static List<AlphaKeyGroup<T>> CreateGroups(IEnumerable<T> items, CultureInfo ci, GetKeyDelegate getKey, bool sort)
            {
                SortedLocaleGrouping slg = new SortedLocaleGrouping();
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
            /// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping.
            /// </summary>
            /// <param name="slg">The </param>
            /// <returns>Theitems source for a LongListSelector</returns>
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