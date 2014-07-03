//-----------------------------------------------------------------------
// <copyright file="AkvelonContactsViewController.cs" company="Akvelon">
//     Copyright (c) Akvelon. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AkvelonContacts.iOS
{
    /// <summary>
    /// Main view controller.
    /// </summary>
    public partial class AkvelonContactsViewController : UIViewController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AkvelonContactsViewController"/> class.
        /// </summary>
        /// <param name="handle"></param>
        public AkvelonContactsViewController(IntPtr handle) : base(handle)
        {
        }

        /// <summary>
        /// Cleans up code applications.
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();// Releases the view if it doesn't have a superview.
        }
    }
}