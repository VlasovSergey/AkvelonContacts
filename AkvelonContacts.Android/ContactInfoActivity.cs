using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AkvelonContacts.Common;
using Android.Graphics;

namespace AkvelonContacts.Android
{
    [Activity(Label = "ContactInfo")]
    public class ContactInfoActivity : Activity
    {
        private Contact contextContact;

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

            this.Title = contextContact.FullName;

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
        }
    }
}