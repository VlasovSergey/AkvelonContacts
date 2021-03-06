﻿//-----------------------------------------------------------------------
// <copyright file="ContactsJsonParser.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Parses JSON to Contact List.
    /// </summary>
    public class ContactsJsonParser : JsonArrayParser<Contact>
    {
        /// <summary>
        /// Converts JObject to Contact object.
        /// </summary>
        /// <param name="jo">JObject for converting.</param>
        /// <returns>Contact object.</returns>
        public override Contact ConvertJObjectToCustomType(Newtonsoft.Json.Linq.JObject jo)
        {
            var c = new Contact();

            c.Dislocation = (string)jo["Dislocation"];
            c.FirstName = (string)jo["FirstName"];
            c.Id = (string)jo["Id"];
            c.LastName = (string)jo["LastName"];
            c.Mail = (string)jo["Mail"];
            c.Skype = (string)jo["Skype"];
            c.Phone = (string)jo["Telephone"];
            c.SecurityKey = (bool)jo["SecurityKey"];
            return c;
        }
    }
}
