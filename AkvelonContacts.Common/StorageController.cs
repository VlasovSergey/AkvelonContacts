//-----------------------------------------------------------------------
// <copyright file="StorageController.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Writes and reads data.
    /// </summary>
    public static class StorageController
    {
        /// <summary>
        /// Writes string to file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <param name="content">Content for write.</param>
        public static void WriteString(string fileName, string content)
        {
            Stream s = new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, IsolatedStorageFile.GetUserStoreForApplication());
            StreamWriter sw = new StreamWriter(s);
            sw.Write(content);
            sw.Close();
        }

        /// <summary>
        /// Reads string from file.
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>File content.</returns>
        public static string ReadString(string fileName)
        {
            string text;

            Stream s = new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, IsolatedStorageFile.GetUserStoreForApplication());
            StreamReader sw = new StreamReader(s);
            text = sw.ReadToEnd();
            sw.Close();

            return text;
        }
    }
}
