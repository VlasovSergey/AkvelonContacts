//-----------------------------------------------------------------------
// <copyright file="AkvelonContactsViewController.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using AkvelonContacts.Common;
using MonoTouch.UIKit;

namespace AkvelonContacts.iOS
{
    /// <summary>
    /// Main view controller.
    /// </summary>
    public partial class AkvelonContactsViewController : UIViewController
    {
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
        /// Initializes a new instance of the <see cref="AkvelonContactsViewController"/> class.
        /// </summary>
        /// <param name="handle">To be added.</param>
        public AkvelonContactsViewController(IntPtr handle)
            : base(handle)
        {
        }

        /// <summary>
        /// Cleans up code applications.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        /// <summary>
        /// To be added.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.displayOnlyContactsWithKey = false;

            this.applicationCtrl = new ApplicationController();

            this.LoadContactsAndDisplay();
        }

        /// <summary>
        /// Called every time any photo loaded.
        /// </summary>
        /// <param name="c">Contact which downloaded photo.</param>
        private void OnLoadPhoto(Contact c)
        {
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
            this.InvokeOnMainThread(() =>
            {
                if (contactList != null)
                {
                    this.contactList = contactList;
                    DisplayContactList(this.contactList, null);
                    this.DisplayTimeUpdate();
                }
                else
                {
                }
            });
        }

        /// <summary>
        /// Shows progress indicator.
        /// </summary>
        private void ShowProgressIndicator()
        {
        }

        /// <summary>
        /// Hides progress indicator.
        /// </summary>
        private void HideProgressIndicator()
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

            // updateTimeTextBlock.Text = TimeConverter.GetElapsedTime((DateTimeOffset)updateTimeOrNull);
        }

        /// <summary>
        /// Displays contact list.
        /// </summary>
        /// <param name="contactList">Contact list for display.</param>
        /// <param name="contactFilter">Filter for contacts.</param>
        private void DisplayContactList(List<Contact> contactList, Func<Contact, bool> contactFilter)
        {
            var newList = ContactsFilter.FilterContacts(contactList, this.displayOnlyContactsWithKey, contactFilter);

            this.ContactsTableView.Source = new TableSource(newList);            
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
        /// Called when the button is clicked, search.
        /// </summary>
        /// <param name="sender">Is a parameter called event sender.</param>
        /// <param name="e">Cancel event args.</param>
        private void Search_Click(object sender, EventArgs e)
        {
            if (this.contactList == null)
            {
                // MessageBox.Show(AppResources.MessageIfSearchWithoutContacts, AppResources.WarningTitle, MessageBoxButton.OK);
            }

            // this.ShowSearchTextBox();
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
    }
}