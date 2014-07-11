﻿//-----------------------------------------------------------------------
// <copyright file="FileDownloader.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="action">Action when download complete.</param>
        public static void DownloadStringAsync(string url, Action<string> action)
        {
            DownloadStreamAsync(
                url,
                (stream) =>
                {
                    if (stream == null)
                    {
                        action(null);
                        return;
                    }

                    StreamReader streamReader = new StreamReader(stream);
                    action(streamReader.ReadToEnd());
                    streamReader.Close();
                });
        }

        /// <summary>
        /// Download stream.
        /// </summary>
        /// <param name="url">URL for download.</param>
        /// <param name="action">Action when download complete.</param>
        public static void DownloadStreamAsync(string url, Action<Stream> action)
        {
            var httpReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            httpReq.BeginGetResponse(
                (ar) =>
                {
                    try
                    {
                        var request = (HttpWebRequest)ar.AsyncState;
                        using (var response = (HttpWebResponse)request.EndGetResponse(ar))
                        {
                            action(response.GetResponseStream());
                        }
                    }
                    catch
                    {
                        action(null);
                    }
                },
            httpReq);
        }
    }
}
