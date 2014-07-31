//-----------------------------------------------------------------------
// <copyright file="JsonArrayParser.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Parses JSON to Custom class.
    /// </summary>
    /// <typeparam name="T">Class custom object.</typeparam>
    public abstract class JsonArrayParser<T>
    {
        /// <summary>
        /// Gets the  <see cref="JsonArrayParser{T}"/> list from JSON string. Returns null if not parse JSON.
        /// </summary>
        /// <param name="json">JSON string.</param>
        /// <returns>Contacts list</returns>
        public List<T> GetListFromJsonArray(string json)
        {
            try
            {
                List<T> objectList = new List<T>();
                var ja = JArray.Parse(json);

                foreach (JObject jo in ja)
                {
                    T c = this.ConvertJObjectToCustomType(jo);
                    objectList.Add(c);
                }

                return objectList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts JObject to <see cref="{T}"/>.
        /// </summary>
        /// <param name="jo">JObject with contact data.</param>
        /// <returns>Convert contact.</returns>
        public abstract T ConvertJObjectToCustomType(JObject jo);
    }
}
