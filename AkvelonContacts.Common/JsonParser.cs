//-----------------------------------------------------------------------
// <copyright file="JsonParser.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Parses JSON to Custom class.
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Parses JSON string to object instance.
        /// </summary>
        /// <typeparam name="T">type of the object.</typeparam>
        /// <param name="json">JSON string representation of the object.</param>
        /// <returns>Deserialized object instance.</returns>
        public static T Deserialize<T>(string json)
        {
            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            object result = null;

            using (MemoryStream mem = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                result = deserializer.ReadObject(mem);
            }

            return (T)result;
        }
    }
}
