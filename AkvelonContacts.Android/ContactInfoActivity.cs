//-----------------------------------------------------------------------
// <copyright file="ContactInfoActivity.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AkvelonContacts.Common;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AkvelonContacts.Android
{
    /// <summary>
    /// Activity with contact info.
    /// </summary>
    [Activity(Label = "ContactInfo")]
    public class ContactInfoActivity : Activity
    {
        /// <summary>
        /// Contains text that is displayed if there is no phone, an e-mail, skype or dislocation.
        /// </summary>
        private string textIfNotData = "Unknown";

        /// <summary>
        /// Contact for activity.
        /// </summary>
        private Contact contextContact;

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

            this.SetContentView(Resource.Layout.ContactInfo);

            this.contextContact = new Contact();
            this.contextContact.FirstName = Intent.GetStringExtra("FirstName");
            this.contextContact.LastName = Intent.GetStringExtra("LastName");
            this.contextContact.Phone = Intent.GetStringExtra("Phone");
            this.contextContact.Mail = Intent.GetStringExtra("Mail");
            this.contextContact.Skype = Intent.GetStringExtra("Skype");
            this.contextContact.Dislocation = Intent.GetStringExtra("Dislocation");
            this.contextContact.Id = Intent.GetStringExtra("Id");
            this.contextContact.SecurityKey = Intent.GetBooleanExtra("SecurityKey", false);

            this.Title = this.contextContact.FullName;

            ImageButton callButton = this.FindViewById<ImageButton>(Resource.Id.callButton);
            ImageButton addContactButton = this.FindViewById<ImageButton>(Resource.Id.addContactButton);
            ImageButton sendSmsButton = this.FindViewById<ImageButton>(Resource.Id.sendSmsButton);
            ImageButton sendEmailButton = this.FindViewById<ImageButton>(Resource.Id.sendEmailButton);

            TextView phoneTextView = this.FindViewById<TextView>(Resource.Id.phoneText);
            TextView emailTextView = this.FindViewById<TextView>(Resource.Id.emileText);

            if (string.IsNullOrEmpty(this.contextContact.Phone))
            {
                phoneTextView.Text = this.textIfNotData;

                callButton.Visibility = ViewStates.Gone;
                sendSmsButton.Visibility = ViewStates.Gone;
            }
            else
            {
                phoneTextView.Text = this.contextContact.Phone;
            }

            if (string.IsNullOrEmpty(this.contextContact.Mail))
            {
                emailTextView.Text = this.textIfNotData;

                sendEmailButton.Visibility = ViewStates.Gone;
            }
            else
            {
                emailTextView.Text = this.contextContact.Mail;
            }

            this.FindViewById<TextView>(Resource.Id.skypeText).Text = !string.IsNullOrEmpty(this.contextContact.Skype) ? this.contextContact.Skype : this.textIfNotData;
            this.FindViewById<TextView>(Resource.Id.dislocationText).Text = !string.IsNullOrEmpty(this.contextContact.Dislocation) ? this.contextContact.Dislocation : this.textIfNotData;

            if (StorageController.FileExists(ApplicationController.GetImagePathByContactId(this.contextContact.Id)))
            {
                Bitmap bmp;
                using (var stream = ApplicationController.GetImageStreamByContactId(this.contextContact.Id))
                {
                    bmp = BitmapFactory.DecodeStream(stream);
                    this.FindViewById<ImageView>(Resource.Id.photoImage).SetImageBitmap(bmp);
                }
            }

            if (this.contextContact.SecurityKey)
            {
                this.FindViewById<ImageView>(Resource.Id.keyImageOfContactInfoView).Visibility = ViewStates.Visible;
            }

            callButton.Click += (s, e) =>
            {
                CommunicationFunctions.CallToContact(this.contextContact, this);
            };

            addContactButton.Click += (s, e) =>
            {
                CommunicationFunctions.AddContact(this.contextContact, this);
            };

            sendSmsButton.Click += (s, e) =>
            {
                CommunicationFunctions.SendSMSToContact(this.contextContact, this);
            };

            sendEmailButton.Click += (s, e) =>
            {
                CommunicationFunctions.SendEmailToContact(this.contextContact, this);
            };
        }
    }
}