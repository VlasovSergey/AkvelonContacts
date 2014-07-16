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
using System.Reflection;
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
        /// <returns>Physical file path for write.</returns>
        public static string WriteString(string fileName, string content)
        {
            IsolatedStorageFileStream s = new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, IsolatedStorageFile.GetUserStoreForApplication());
            StreamWriter sw = new StreamWriter(s);
            sw.Write(content);
            sw.Close();
            return s.Name;
        }

        /// <summary>
        /// Reads string from file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File content.</returns>
        public static string ReadString(string fileName)
        {
            string text;

            IsolatedStorageFileStream s = GetStreamOfFileForRead(fileName);
            StreamReader sw = new StreamReader(s);
            text = sw.ReadToEnd();
            sw.Close();

            return text;
        }

        /// <summary>
        /// Writes stream to file. 
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="streamForSave">Stream for write.</param>
        /// <returns>Physical file path for write.</returns>
        public static string WriteStream(string fileName, Stream streamForSave)
        {
            IsolatedStorageFileStream s = new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, IsolatedStorageFile.GetUserStoreForApplication());
            streamForSave.CopyTo(s);
            s.Close();
            return s.Name;
        }

        /// <summary>
        /// Gets stream from file for read.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File stream.</returns>
        public static IsolatedStorageFileStream GetStreamOfFileForRead(string fileName)
        {
            IsolatedStorageFileStream s = new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, IsolatedStorageFile.GetUserStoreForApplication());
            return s;
        }

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="fileName">The file to check.</param>
        /// <returns>Result check.</returns>
        public static bool FileExists(string fileName)
        {
            return IsolatedStorageFile.GetUserStoreForApplication().FileExists(fileName);
        }

        /// <summary>
        /// Gets physical path for file from local storage.
        /// </summary>
        /// <param name="filePath">Local path.</param>
        /// <returns>Physical path.</returns>
        public static string GetPhysicalPathForLocalFilePath(string filePath)
        {
            string physicalPath = null;

            if (FileExists(filePath))
            {
                IsolatedStorageFileStream s = new IsolatedStorageFileStream(filePath, FileMode.Open, FileAccess.Read, IsolatedStorageFile.GetUserStoreForApplication());
                s.Close();
                physicalPath = s.Name;
            }

            return physicalPath;
        }

        /// <summary>
        /// Creates a directory in the isolated storage scope.
        /// </summary>
        /// <param name="directoryName">The relative path of the directory to create within the isolated storage.</param>
        public static void CreateDirectory(string directoryName)
        {
            IsolatedStorageFile.GetUserStoreForApplication().CreateDirectory(directoryName);
        }

        /// <summary>
        /// Determines whether the specified path refers to an existing directory in
        /// the isolated store.
        /// </summary>
        /// <param name="directoryName">The path to test.</param>
        /// <returns>
        /// true if path refers to an existing directory in the isolated store and is
        /// not null; otherwise, false.
        /// </returns>
        public static bool DirectoryExists(string directoryName)
        {
            return IsolatedStorageFile.GetUserStoreForApplication().DirectoryExists(directoryName);
        }
    }
}