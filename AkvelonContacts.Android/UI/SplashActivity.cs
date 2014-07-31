//-----------------------------------------------------------------------
// <copyright file="SplashActivity.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Threading;
using AkvelonContacts.Android;
using Android.App;
using Android.OS;

namespace SplashScreen
{
    /// <summary>
    /// Splash screen.
    /// </summary>
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
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
            Thread.Sleep(1000); // Simulate a long loading process on app startup.
            this.StartActivity(typeof(MainActivity));
        }
    }
}