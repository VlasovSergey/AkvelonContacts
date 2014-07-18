//-----------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using AkvelonContacts.Common;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AkvelonContacts.Android
{
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = "AkvelonContacts.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Contains contact list.
        /// </summary>
        private List<Contact> contactList;

        /// <summary>
        /// Application controller.
        /// </summary>
        private ApplicationController applicationCtrl;

        /// <summary>
        /// Contact ListView.
        /// </summary>
        private ListView contactListView;

        /// <summary>
        /// Called when the activity is starting.
        /// </summary>
        /// <param name="bundle">
        /// If the activity is being re-initialized after previously being shut down
        /// then this Bundle contains the data it most recently supplied in Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle).
        /// Note: Otherwise it is null.
        /// </param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            
            this.SetContentView(Resource.Layout.Main); // Set our view from the "main" layout resource

            this.applicationCtrl = new ApplicationController();

            this.contactListView = this.FindViewById<ListView>(Resource.Id.contactListView);

            this.applicationCtrl.GetContacts(
                (contactList) => {
                    this.contactList = contactList;
                    this.DisplayContactList(this.contactList);
                    
                },
                (contact) => {
                }
                );
            this.LoadAndShowContactList();
        }

        private void DisplayContactList(List<Contact> contactList)
        {
            this.BindContactList(contactList, this.contactListView);
        }

        /// <summary>
        /// Loads and shows contact list.
        /// </summary>
        private void LoadAndShowContactList()
        {
            if (this.IsAvailableInternet())
            {
                this.applicationCtrl.DownloadContactList((List<Contact> result) =>
                {
                    if (result != null)
                    {
                        this.BindContactList(result, contactListView);
                        return;
                    }
                    else
                    {
                        this.BindContactList(this.applicationCtrl.LoadLocalContactList(), this.contactListView);
                    }
                });
            }
            else
            {
                this.BindContactList(this.applicationCtrl.LoadLocalContactList(), this.contactListView);
            }
        }

        /// <summary>
        /// Binds to contactList ListView.
        /// </summary>
        /// <param name="contactList">Contacts list for binding.</param>
        /// <param name="contactListView">ListView for binding.</param>
        private void BindContactList(List<Contact> contactList, ListView contactListView)
        {
            this.RunOnUiThread(() =>
            {
                var listAdapter = new ContactScreenAdapter(this, contactList);
                contactListView.Adapter = listAdapter;
                listAdapter.NotifyDataSetChanged();
            });
        }

        /// <summary>
        /// Gets availability for Internet.
        /// </summary>
        /// <returns>Availability for Internet.</returns>
        private bool IsAvailableInternet()
        {
            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            var activeConnection = connectivityManager.ActiveNetworkInfo;

            return (activeConnection != null) && activeConnection.IsConnected;
        }

        /// <summary>
        /// Class adapter for communications ContactListView with Contacts List of.
        /// </summary>
        internal partial class ContactScreenAdapter : BaseAdapter<Contact>
        {
            /// <summary>
            /// Contains items for display.
            /// </summary>
            private List<Contact> items;

            /// <summary>
            /// Context activity.
            /// </summary>
            private Activity context;

            /// <summary>
            /// Initializes a new instance of the <see cref="ContactScreenAdapter" /> class.
            /// </summary>
            /// <param name="context">Context activity.</param>
            /// <param name="items">Contacts List.</param>
            public ContactScreenAdapter(Activity context, List<Contact> items)
                : base()
            {
                this.context = context;
                this.items = items;
            }

            /// <summary>
            /// Gets contacts count.
            /// </summary>
            public override int Count
            {
                get { return this.items.Count; }
            }

            /// <summary>
            /// Gets contacts by index.
            /// </summary>
            /// <param name="position">Contact index.</param>
            /// <returns>Contact by index.</returns>
            public override Contact this[int position]
            {
                get { return this.items[position]; }
            }

            /// <summary>
            /// Gets id item.
            /// </summary>
            /// <param name="position">Contact index.</param>
            /// <returns>Id for item.</returns>
            public override long GetItemId(int position)
            {
                return position;
            }

            /// <summary>
            /// Gets view for a contact.
            /// </summary>
            /// <param name="position">Position in list.</param>
            /// <param name="convertView">Convert view.</param>
            /// <param name="parent">Parent by view.</param>
            /// <returns>View with contact data</returns>
            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var item = this.items[position];
                View view = convertView;

                if (view == null)
                {
                    view = this.context.LayoutInflater.Inflate(Resource.Layout.ContactListViewTemplate, null);
                }

                view.FindViewById<TextView>(Resource.Id.contactName).Text = item.FullName;

                return view;
            }
        }
    }
}