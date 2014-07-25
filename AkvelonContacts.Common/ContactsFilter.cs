//-----------------------------------------------------------------------
// <copyright file="ContactsFilter.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Contains met
    /// </summary>
    public static class ContactsFilter
    {
        /// <summary>
        /// Filters contacts.
        /// </summary>
        /// <param name="contactList">Contact list for display.</param>
        /// <param name="contactsOnlyWithKay">Value indicating whether display contacts only with key.</param>
        /// <param name="contactFilter">Filter for contacts.</param>
        /// <returns>Filtered contacts.</returns>
        public static List<Contact> FilterContacts(List<Contact> contactList, bool contactsOnlyWithKay, Func<Contact, bool> contactFilter)
        {
            if (contactFilter != null)
            {
                return FilterContacts(
                    contactList,
                    (cont) =>
                    {
                        return (contactsOnlyWithKay ? cont.SecurityKey : true) && contactFilter(cont);
                    });
            }

            if (contactsOnlyWithKay)
            {
                return FilterContacts(contactList, (Contact c) => { return c.SecurityKey; });
            }

            return contactList;
        }

        /// <summary>
        /// Filters contacts.
        /// </summary>
        /// <param name="contactList">Contacts for filtering.</param>
        /// <param name="contactFilter">Filter for filtering.</param>
        /// <returns>Filtered contacts.</returns>
        private static List<Contact> FilterContacts(List<Contact> contactList, Func<Contact, bool> contactFilter)
        {
            var newList = contactList.Where(item => contactFilter(item)).ToList<Contact>();
            return newList;
        }
    }
}
