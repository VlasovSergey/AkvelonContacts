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

            this.FindViewById<TextView>(Resource.Id.phoneText).Text = this.contextContact.Phone;
            this.FindViewById<TextView>(Resource.Id.emileText).Text = this.contextContact.Mail;
            this.FindViewById<TextView>(Resource.Id.skypeText).Text = this.contextContact.Skype;
            this.FindViewById<TextView>(Resource.Id.dislocationText).Text = this.contextContact.Dislocation;

            if (StorageController.FileExists(ApplicationController.GetImagePathByContactId(this.contextContact.Id)))
            {
                Bitmap bmp;
                using (var stream = ApplicationController.GetImageStreamByContactId(this.contextContact.Id))
                {
                    bmp = BitmapFactory.DecodeStream(stream);
                    this.FindViewById<ImageView>(Resource.Id.photoImage).SetImageBitmap(bmp);
                }
            }

            this.FindViewById<Button>(Resource.Id.callButton).Click += (s, e) =>
            {
                CommunicationFunctions.CallToContact(this.contextContact, this);
            };
        }
    }
}