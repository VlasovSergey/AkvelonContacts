﻿//-----------------------------------------------------------------------
// <copyright file="TimeConverter.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace AkvelonContacts.Common
{
    /// <summary>
    /// Class for work with time.
    /// </summary>
    public static class TimeConverter
    {
        /// <summary>
        /// Gets the time since the last update.
        /// </summary>
        /// <param name="lastWriteTime">Date for receipt of elapsed time.</param>
        /// <returns>Time since the last update.</returns>
        public static string GetElapsedTime(DateTimeOffset lastWriteTime)
        {
            string text = string.Empty;
            
            TimeSpan updateTime = TimeSpan.FromTicks(DateTimeOffset.Now.Ticks - lastWriteTime.Ticks);

            if (updateTime.Days != 0)
            {
                text += updateTime.Days + " Days ";
            }

            if (updateTime.Hours != 0)
            {
                text += updateTime.Hours + " Hours ";
            }

            if (updateTime.Minutes != 0)
            {
                text += updateTime.Minutes + " Min ";
            }

            if (updateTime.Seconds != 0)
            {
                text += updateTime.Seconds + " Sec ";
            }

            if (text != string.Empty)
            {
                text = "Updated " + text + "ago";
            }
            else
            {
                text = "Now updated";
            }

            return text;
        }
    }
}
