//-----------------------------------------------------------------------
// <copyright file="ContactScreenAdapter.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Class adapter for communications ContactListView with Contacts List of.
    /// </summary>
    public class ContactScreenAdapter : BaseAdapter<Contact>
    {
        /// <summary>
        /// Contains items for display.
        /// </summary>
        private List<ContactListViewItem> items;

        /// <summary>
        /// Context activity.
        /// </summary>
        private Activity context;

        /// <summary>
        /// Contains photo bitmaps.
        /// </summary>
        private Dictionary<string, Bitmap> photos;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactScreenAdapter" /> class.
        /// </summary>
        /// <param name="context">Context activity.</param>
        /// <param name="contacts">Contacts List.</param>
        /// <param name="ci">The CultureInfo to sort by.</param>
        public ContactScreenAdapter(Activity context, List<Contact> contacts, CultureInfo ci)
            : base()
        {
            this.context = context;

            this.SortContacts(contacts, ci);
            string previousLetter = null;

            var newItems = new List<ContactListViewItem>();

            foreach (Contact contact in contacts)
            {
                string firstLetter = contact.FirstName.Substring(0, 1).ToUpper();

                // Group numbers together in the scroller
                if (!char.IsLetter(firstLetter, 0))
                {
                    firstLetter = "#";
                }

                // If we've changed to a new letter, add the previous letter to the alphabet scroller
                if (previousLetter == null || firstLetter != previousLetter)
                {
                    previousLetter = firstLetter;
                    newItems.Add(new ContactListViewItem(true, firstLetter, null));
                }

                newItems.Add(new ContactListViewItem(false, firstLetter, contact));
            }

            this.items = newItems;

            this.photos = new Dictionary<string, Bitmap>();
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
            get { return this.items[position].Contact; }
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

            if (item.IsHeader)
            {
                view = this.context.LayoutInflater.Inflate(Resource.Layout.ContactListViewItemHeader, null);

                view.FindViewById<TextView>(Resource.Id.itemHeaderText).Text = item.KeyGroup;
            }
            else
            {
                Contact contact = item.Contact;
                view = this.context.LayoutInflater.Inflate(Resource.Layout.ContactListViewTemplate, null);

                view.FindViewById<TextView>(Resource.Id.contactName).Text = contact.FullName;

                if (contact.SecurityKey)
                {
                    view.FindViewById<ImageView>(Resource.Id.keyImageOfItem).Visibility = ViewStates.Visible;
                }

                ImageView photoView = view.FindViewById<ImageView>(Resource.Id.contactPhoto);

                if (this.photos.ContainsKey(contact.Id))
                {
                    photoView.SetImageBitmap(this.photos[contact.Id]);
                    return view;
                }

                if (StorageController.FileExists(ApplicationController.GetImagePathByContactId(contact.Id)))
                {
                    using (var stream = ApplicationController.GetImageStreamByContactId(contact.Id))
                    {
                        try
                        {
                            if (stream.Length != 0)
                            {
                                Bitmap bmp;
                                bmp = BitmapFactory.DecodeStream(stream);
                                photoView.SetImageBitmap(bmp);
                                this.photos.Add(contact.Id, bmp);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Failed to get a picture");
                        }
                    }
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
                    return ci.CompareInfo.Compare(c0.FirstName, c1.FirstName);
                });
        }

        /// <summary>
        /// Item for list view.
        /// </summary>
        private class ContactListViewItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ContactListViewItem" /> class.
            /// </summary>
            /// <param name="isHeader">Denotes a header or not.</param>
            /// <param name="keyGroup">Key group.</param>
            /// <param name="contact">Contact of item.</param>
            public ContactListViewItem(bool isHeader, string keyGroup, Contact contact)
            {
                this.IsHeader = isHeader;
                this.KeyGroup = keyGroup;
                this.Contact = contact;
            }

            /// <summary>
            /// Gets or sets key group.
            /// </summary>
            public string KeyGroup { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether is header.
            /// </summary>
            public bool IsHeader { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether item heading.
            /// </summary>
            public Contact Contact { get; set; }
        }
    }
}