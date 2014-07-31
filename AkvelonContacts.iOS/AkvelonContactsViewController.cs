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

            this.applicationCtrl = new ApplicationController();

            this.LoadContactsAndDisplay();
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
                    ContactsTableView.Source = new TableSource(contactList);
                }
                else
                {
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
    }
}