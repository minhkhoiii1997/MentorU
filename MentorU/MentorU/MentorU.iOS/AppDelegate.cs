﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Foundation;
using Microsoft.Identity.Client;
using Microsoft.WindowsAzure.MobileServices;
using UIKit;

namespace MentorU.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            CurrentPlatform.Init();

            LoadApplication(new App());

            //AppCenter.Start("fb0078a5-c257-43b7-b61d-735790293192",
            //       typeof(Analytics), typeof(Crashes));

            return base.FinishedLaunching(app, options);
        }
    }
}
