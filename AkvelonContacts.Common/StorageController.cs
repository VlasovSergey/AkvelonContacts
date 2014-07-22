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
        public static string WriteStringToFile(string fileName, string content)
        {
            try
            {
                IsolatedStorageFileStream s = new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, IsolatedStorageFile.GetUserStoreForApplication());
                StreamWriter sw = new StreamWriter(s);
                sw.Write(content);
                sw.Close();
                s.Close();

                return s.Name;
            }
            catch
            {
                Console.WriteLine("Unable to write string to file");
                return null;
            }
        }

        /// <summary>
        /// Reads string from file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File content.</returns>
        public static string ReadAsString(string fileName)
        {
            try
            {
                string text = null;

                IsolatedStorageFileStream s = GetStreamOfFileForRead(fileName);
                StreamReader sw = new StreamReader(s);
                try
                {
                    text = sw.ReadToEnd();
                }
                finally
                {
                    sw.Close();
                    s.Close();
                }

                return text;
            }
            catch
            {
                Console.WriteLine("Unable to read string to file");
                return null;
            }
        }

        /// <summary>
        /// Writes stream to file. 
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <param name="streamForSave">Stream for write.</param>
        /// <returns>Physical file path for write.</returns>
        public static string CopyStreamToLocalStore(string fileName, Stream streamForSave)
        {
            try
            {
                IsolatedStorageFileStream s = new IsolatedStorageFileStream(fileName, FileMode.Create, FileAccess.Write, IsolatedStorageFile.GetUserStoreForApplication());
                streamForSave.CopyTo(s);
                s.Close();
                return s.Name;
            }
            catch
            {
                Console.WriteLine("Unable to copy the stream to the local store.");
                return null;
            }
        }

        /// <summary>
        /// Gets stream from file for read.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>File stream.</returns>
        public static IsolatedStorageFileStream GetStreamOfFileForRead(string fileName)
        {
            try
            {
                IsolatedStorageFileStream s = new IsolatedStorageFileStream(fileName, FileMode.Open, FileAccess.Read, IsolatedStorageFile.GetUserStoreForApplication());
                return s;
            }
            catch
            {
                Console.WriteLine("Unable to get stream from file for read.");
                return null;
            }
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
            try
            {
                if (!FileExists(filePath))
                {
                    return null;
                }

                IsolatedStorageFileStream s = new IsolatedStorageFileStream(filePath, FileMode.Open, FileAccess.Read, IsolatedStorageFile.GetUserStoreForApplication());
                s.Close();
                return s.Name;
            }
            catch
            {
                Console.WriteLine("Unable to get physical path for file from local storage.");
                return null;
            }
        }

        /// <summary>
        /// Creates a directory in the isolated storage scope.
        /// </summary>
        /// <param name="directoryName">The relative path of the directory to create within the isolated storage.</param>
        public static void CreateDirectory(string directoryName)
        {
            try
            {
                IsolatedStorageFile.GetUserStoreForApplication().CreateDirectory(directoryName);
            }
            catch
            {
                Console.WriteLine("Unable to create a directory in the isolated storage scope.");
            }
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

        /// <summary>
        /// Returns the date and time a specified file or directory was last written to.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <returns>Last write time.</returns>
        public static DateTimeOffset? GetLastWriteTime(string filePath)
        {
            if (!FileExists(filePath))
            {
                return null;
            }

            return IsolatedStorageFile.GetUserStoreForApplication().GetLastWriteTime(filePath);
        }
    }
}