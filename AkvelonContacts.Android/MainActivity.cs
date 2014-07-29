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
    [Activity(Label = "AkvelonContacts.Android", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
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
        private ImageButton searchButton;

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
        /// Progress dialog.
        /// </summary>
        private ProgressDialog progressDialog;

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

            this.searchButton = this.FindViewById<ImageButton>(Resource.Id.searchButton);
            this.searchTextView = this.FindViewById<EditText>(Resource.Id.searchText);
            this.title = this.FindViewById<TextView>(Resource.Id.title);
            this.footer = this.FindViewById<LinearLayout>(Resource.Id.footer);
            this.contactListView = this.FindViewById<ListView>(Resource.Id.contactListView);

            this.LoadContactsAndDisplay();

            this.searchButton.Click += (s, e) =>
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
                this.HideKeyboard();

                var adapter = (ContactScreenAdapter)this.contactListView.Adapter;

                var item = adapter[(int)e.Id];

                if (item == null)
                {
                    return;
                }

                Intent intent = new Intent(this, typeof(ContactInfoActivity));
                intent.PutExtra("Phone", item.Phone);
                intent.PutExtra("Skype", item.Skype);
                intent.PutExtra("Mail", item.Mail);
                intent.PutExtra("FirstName", item.FirstName);
                intent.PutExtra("LastName", item.LastName);
                intent.PutExtra("SecurityKey", item.SecurityKey);
                intent.PutExtra("Dislocation", item.Dislocation);
                intent.PutExtra("Id", item.Id);
                this.StartActivity(intent);
            };

            this.FindViewById<ImageButton>(Resource.Id.refreshButton).Click += (s, e) =>
            {
                this.DownloadContactsAndDysplay();
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

            this.ShowKeyboard();
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
        /// Shows keyboard.;
        /// </summary>
        private void ShowKeyboard()
        {
            InputMethodManager inputMethodManager = this.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(this.searchTextView, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }

        /// <summary>
        /// Hides keyboard.
        /// </summary>
        private void HideKeyboard()
        {
            (GetSystemService(Context.InputMethodService) as InputMethodManager).HideSoftInputFromWindow(this.searchTextView.WindowToken, HideSoftInputFlags.None);
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
            this.ShowProgressDialog("Loading contacts. Please wait...");
            this.applicationCtrl.GetContacts(this.OnLoadContactList, this.OnLoadPhoto);
        }

        /// <summary>
        /// Called when contact list is loaded without Photo.
        /// </summary>
        /// <param name="contactList">Contact list.</param>
        private void OnLoadContactList(List<Contact> contactList)
        {
            this.RunOnUiThread(() =>
            {
                if (contactList == null)
                {
                    ShowMessage("Could not load contacts. Please check your internet connection.", "Warning");
                }
                else
                {
                    this.contactList = contactList;
                    this.DisplayContactList(this.contactList, null);
                }

                this.HideProgressDialog();
            });
        }

        /// <summary>
        /// Called every time any photo loaded.
        /// </summary>
        /// <param name="c">Contact which downloaded photo.</param>
        private void OnLoadPhoto(Contact c) { }

        /// <summary>
        /// Loads and display contacts.
        /// </summary>
        private void DownloadContactsAndDysplay()
        {
            this.ShowProgressDialog("Loading contacts. Please wait...");
            this.applicationCtrl.DownloadContactsAndPhotos(this.OnLoadContactList, this.OnLoadPhoto);
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
            var listAdapter = new ContactScreenAdapter(this, contactList, new CultureInfo("ru-RU"));
            contactListView.Adapter = listAdapter;
            listAdapter.NotifyDataSetChanged();
        }

        /// <summary>
        /// Finds and displays contacts for text.
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
        /// Show progress dialog.
        /// </summary>
        /// <param name="title">Title for progress dialog.</param>
        private void ShowProgressDialog(string title)
        {
            this.progressDialog = new ProgressDialog(this);
            this.progressDialog.Indeterminate = true;
            this.progressDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            this.progressDialog.SetMessage(title);
            this.progressDialog.SetCancelable(false);
            this.progressDialog.Show();
        }

        /// <summary>
        /// Progress dialog.
        /// </summary>
        private void HideProgressDialog()
        {
            this.progressDialog.Cancel();
        }
    }
}