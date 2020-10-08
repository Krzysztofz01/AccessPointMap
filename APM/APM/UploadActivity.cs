using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace APM
{
    [Activity(Label = "UploadActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UploadActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Prevent app from sleeping
            Window.SetFlags(Android.Views.WindowManagerFlags.KeepScreenOn, Android.Views.WindowManagerFlags.KeepScreenOn);

            //Init the activity and view
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_upload);
        }
    }
}