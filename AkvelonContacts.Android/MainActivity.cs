//-----------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using AkvelonContacts.Common;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
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
        /// Search button.
        /// </summary>
        private ImageButton seatchButton;

        /// <summary>
        /// Edit text for search.
        /// </summary>
        private EditText searchTextView;

        /// <summary>
        /// Title text.
        /// </summary>
        private TextView title;

        /// <summary>
        /// Footer linear layout.
        /// </summary>
        private LinearLayout footer;

        /// <summary>
        /// Gets or sets a value indicating whether display contacts only with key.
        /// </summary>
        private bool displayOnlyContactsWithKey;

        /// <summary>
        /// Called when the activity has detected the user's press of the back key.
        /// </summary>
        public override void OnBackPressed()
        {
            if (this.searchTextView.Visibility == ViewStates.Visible)
            {
                this.HideSearch();
            }
            else
            {
                base.OnBackPressed();
            }
        }

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

            this.displayOnlyContactsWithKey = false;

            this.seatchButton = this.FindViewById<ImageButton>(Resource.Id.searchButton);
            this.searchTextView = this.FindViewById<EditText>(Resource.Id.searchText);
            this.title = this.FindViewById<TextView>(Resource.Id.title);
            this.footer = this.FindViewById<LinearLayout>(Resource.Id.footer);
            this.contactListView = this.FindViewById<ListView>(Resource.Id.contactListView);

            this.LoadContactsAndDisplay();

            this.seatchButton.Click += (s, e) =>
            {
                if (contactList == null)
                {
                    ShowMessage("Contact list is not available.", "Warning");
                    return;
                }

                ShowSearch();
            };

            this.contactListView.ItemClick += (s, e) =>
            {
                var adapter = (ContactScreenAdapter)this.contactListView.Adapter;

                var contact = adapter[(int)e.Id];
                Intent intent = new Intent(this, typeof(ContactInfoActivity));
                intent.PutExtra("Phone", contact.Phone);
                intent.PutExtra("Skype", contact.Skype);
                intent.PutExtra("Mail", contact.Mail);
                intent.PutExtra("FirstName", contact.FirstName);
                intent.PutExtra("LastName", contact.LastName);
                intent.PutExtra("SecurityKey", contact.SecurityKey);
                intent.PutExtra("Dislocation", contact.Dislocation);
                intent.PutExtra("Id", contact.Id);
                this.StartActivity(intent);
            };

            this.searchTextView.TextChanged += (s, e) => { this.DisplayContactsByText(this.searchTextView.Text); };
        }

        /// <summary>
        /// Shows and set up focus to search edit text view.
        /// </summary>
        private void ShowSearch()
        {
            this.title.Visibility = ViewStates.Gone;
            this.searchTextView.Visibility = ViewStates.Visible;
            this.footer.Visibility = ViewStates.Gone;

            this.searchTextView.RequestFocus();

            InputMethodManager inputMethodManager = this.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(this.searchTextView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        /// <summary>
        /// Hides search edit text view.
        /// </summary>
        private void HideSearch()
        {
            this.title.Visibility = ViewStates.Visible;
            this.searchTextView.Visibility = ViewStates.Gone;
            this.footer.Visibility = ViewStates.Visible;
        }

        /// <summary>
        /// Shows message box.
        /// </summary>
        /// <param name="text">Text of message.</param>
        /// <param name="title">Title of message box.</param>
        private void ShowMessage(string text, string title)
        {
            var builder = new AlertDialog.Builder(this);
            builder.SetMessage(text);
            builder.SetTitle(title);
            builder.SetPositiveButton("OK", (s, e) => { });
            builder.Create().Show();
        }

        /// <summary>
        /// Loads and display contacts.
        /// </summary>
        private void LoadContactsAndDisplay()
        {
            this.applicationCtrl.GetContacts(
                (contactList) =>
                {
                    this.RunOnUiThread(() =>
                    {
                        if (contactList == null)
                        {
                            ShowMessage("Could not load contacts. Please check your internet connection.", "Warning");
                            return;
                        }

                        this.contactList = contactList;
                        this.DisplayContactList(this.contactList, null);
                    });
                },
                (contact) => { });
        }

        /// <summary>
        /// Displays contact list.
        /// </summary>
        /// <param name="contactList">Contact list for display.</param>
        /// <param name="contactFilter">Filter for contacts.</param>
        private void DisplayContactList(List<Contact> contactList, Func<Contact, bool> contactFilter)
        {
            var newList = ContactsFilter.FilterContacts(contactList, this.displayOnlyContactsWithKey, contactFilter);

            this.BindContactList(newList, this.contactListView);
        }

        /// <summary>
        /// Binds to contactList ListView.
        /// </summary>
        /// <param name="contactList">Contacts list for binding.</param>
        /// <param name="contactListView">ListView for binding.</param>
        private void BindContactList(List<Contact> contactList, ListView contactListView)
        {
            var listAdapter = new ContactScreenAdapter(this, contactList, new CultureInfo("ru-RU"), true);
            contactListView.Adapter = listAdapter;
            listAdapter.NotifyDataSetChanged();
        }

        /// <summary>
        /// finds and displays contacts for text.
        /// </summary>
        /// <param name="text">Text for searching.</param>
        private void DisplayContactsByText(string text)
        {
            this.DisplayContactList(
                this.contactList,
                (contact) =>
                {
                    bool mailCriterion = contact.Mail != null && contact.Mail.IndexOf(text, System.StringComparison.OrdinalIgnoreCase) >= 0;
                    bool fullNameCriterion = contact.FullName.IndexOf(text, System.StringComparison.OrdinalIgnoreCase) >= 0;
                    return mailCriterion || fullNameCriterion;
                });
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
            /// <param name="ci">The CultureInfo to sort by.</param>
            /// <param name="sort">Will sort the data if true.</param>
            public ContactScreenAdapter(Activity context, List<Contact> items, CultureInfo ci, bool sort)
                : base()
            {
                this.context = context;
                this.items = items;

                if (sort)
                {
                    this.SortContacts(this.items, ci);
                }
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

                if (StorageController.FileExists(ApplicationController.GetImagePathByContactId(item.Id)))
                {
                    Bitmap bmp;
                    using (var stream = ApplicationController.GetImageStreamByContactId(item.Id))
                    {
                        bmp = BitmapFactory.DecodeStream(stream);
                        view.FindViewById<ImageView>(Resource.Id.contactPhoto).SetImageBitmap(bmp);
                    }
                }

                return view;
            }

            /// <summary>
            /// Sorts items.
            /// </summary>
            /// <param name="contactList">Contact list for short</param>
            /// <param name="ci">Culture info for short.</param>
            private void SortContacts(List<Contact> contactList, CultureInfo ci)
            {
                contactList.Sort(
                    (c0, c1) =>
                    {
                        return ci.CompareInfo.Compare(c0.FullName, c1.FullName);
                    });
            }
        }
    }
}