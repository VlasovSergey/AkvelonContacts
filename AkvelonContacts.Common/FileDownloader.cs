//-----------------------------------------------------------------------
// <copyright file="FileDownloader.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Contains methods for download.
    /// </summary>
    public class FileDownloader
    {
        /// <summary>
        /// Downloads string.
        /// </summary>
        /// <param name="url">URL for download.</param>
        /// <param name="encoding">System.Text.Encoding used to upload and download strings.</param>
        /// <param name="action">Action when download complete.</param>
        public static void DownloadStringAsync(string url, Encoding encoding, Action<string> action)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    action(e.Result);
                }
            };

            if (encoding != null)
            {
                webClient.Encoding = encoding;
            }

            webClient.DownloadStringAsync(new Uri(url));
        }
    }
}
