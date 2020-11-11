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
using APM.Services;

namespace APM
{
    [Activity(Label = "UploadActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UploadActivity : Activity
    {
        private Button buttonUpload;
        private EditText loginForm;
        private EditText passwordForm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Prevent app from sleeping
            Window.SetFlags(Android.Views.WindowManagerFlags.KeepScreenOn, Android.Views.WindowManagerFlags.KeepScreenOn);

            //Init the activity and view
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_upload);

            //Assign GUI elemets to objects
            buttonUpload = FindViewById<Button>(Resource.Id.buttonStartUpload);
            loginForm = FindViewById<EditText>(Resource.Id.inputLogin);
            passwordForm = FindViewById<EditText>(Resource.Id.inputPassword);

            //Assign events to Button elements
            buttonUpload.Click += buttonUploadClickEvent;
        }

        private async void buttonUploadClickEvent(object sender, System.EventArgs e)
        {
            var connectionService = new ConnectionService();
            if (await connectionService.SendData(AccessPoint.AccessPointContainer, loginForm.Text, passwordForm.Text))
            {
                Toast.MakeText(this, "Data successful posted!", ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(this, "Some errors occured while posting data!", ToastLength.Long).Show();
            }
        }
    }
}