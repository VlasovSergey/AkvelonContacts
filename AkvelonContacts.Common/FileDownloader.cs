//-----------------------------------------------------------------------
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
        /// Downloads file as string.
        /// </summary>
        /// <param name="url">URL for download.</param>
        /// <param name="action">Action when download complete.</param>
        public static void DownloadFileAsString(string url, Action<string> action)
        {
            DownloadFile(
                url,
                (stream) =>
                {
                    if (stream == null)
                    {
                        action(null);
                        return;
                    }

                    string result;

                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    action(result);
                });
        }

        /// <summary>
        /// Download file.
        /// </summary>
        /// <param name="url">URL for download.</param>
        /// <param name="action">Action when download complete.</param>
        public static void DownloadFile(string url, Action<Stream> action)
        {
            Stream loadedContactList;
            var httpReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            httpReq.BeginGetResponse(
                (ar) =>
                {
                    try
                    {
                        var request = (HttpWebRequest)ar.AsyncState;
                        using (var response = (HttpWebResponse)request.EndGetResponse(ar))
                        {
                            loadedContactList = response.GetResponseStream();
                            action(loadedContactList);
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
