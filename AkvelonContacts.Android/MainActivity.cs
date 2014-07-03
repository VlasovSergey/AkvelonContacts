//-----------------------------------------------------------------------
// <copyright file="MainActivity.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AkvelonContacts.Android
{   
    /// <summary>
    /// Main activity
    /// </summary>
    [Activity(Label = "AkvelonContacts.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        /// <summary>
        /// Called when the activity is starting.
        /// </summary>
        /// <param name="bundle">
        /// If the activity is being re-initialized after previously being shut down
        /// then this Bundle contains the data it most recently supplied in Android.App.Activity.OnSaveInstanceState(Android.OS.Bundle).
        /// Note: Otherwise it is null.
        /// </param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.SetContentView(Resource.Layout.Main); // Set our view from the "main" layout resource
        }
    }
}