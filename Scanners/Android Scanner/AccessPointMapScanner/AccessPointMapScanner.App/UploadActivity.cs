using System;
using AccessPointMapScanner.App.Utilities;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace AccessPointMapScanner.App
{
    [Activity(Label = "UploadActivity")]
    public class UploadActivity : Activity
    {
        private string serializedAccessPoints;

        private EditText LoginFormInput;
        private EditText PasswordFormInput;
        private Button AuthButton;
        private CheckBox ForceMasterCheckBox;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Keep screen on rule and remove navbar
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            serializedAccessPoints = Intent.GetStringExtra("accesspoints");

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_upload);

            //UI Initialize
            LoginFormInput = FindViewById<EditText>(Resource.Id.formLoginInput);

            PasswordFormInput = FindViewById<EditText>(Resource.Id.formPasswordInput);

            AuthButton = FindViewById<Button>(Resource.Id.authButton);

            ForceMasterCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxForceMaster);

            AuthButton.Click += AuthButtonClickEvent;
        }

        private async void AuthButtonClickEvent(object sender, EventArgs e)
        {
            if(await WebService.SendResults(serializedAccessPoints, LoginFormInput.Text, PasswordFormInput.Text, ForceMasterCheckBox.Checked))
            {
                Toast.MakeText(this, "Data upload successful!", ToastLength.Long).Show();
                return;
            }
            Toast.MakeText(this, "Data upload failed! Uncheck the master option!", ToastLength.Long).Show();
        }
    }
}