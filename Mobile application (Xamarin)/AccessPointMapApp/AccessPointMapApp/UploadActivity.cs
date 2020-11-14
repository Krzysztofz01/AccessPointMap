using System.Collections.Generic;
using AccessPointMapApp.Models;
using AccessPointMapApp.Services;
using Android.App;
using Android.OS;
using Android.Widget;

namespace AccessPointMapApp
{
    [Activity(Label = "UploadActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UploadActivity : Activity
    {
        private EditText LoginFormInput;
        private EditText PasswordFormInput;
        private Button AuthButton;
        private Button StorageButton;
        private CheckBox ForceMasterCheckBox;

        private List<Accesspoint> AccesspointContainer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            Window.SetFlags(Android.Views.WindowManagerFlags.KeepScreenOn, Android.Views.WindowManagerFlags.KeepScreenOn);

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_upload);

            var serializationService = new SerializationService();
            AccesspointContainer = serializationService.DeserializeAccessPointContainer(Intent.GetStringExtra("AccessPoints"));

            LoginFormInput = FindViewById<EditText>(Resource.Id.formLoginInput);
            PasswordFormInput = FindViewById<EditText>(Resource.Id.formPasswordInput);
            AuthButton = FindViewById<Button>(Resource.Id.authButton);
            StorageButton = FindViewById<Button>(Resource.Id.storageButton);
            ForceMasterCheckBox = FindViewById<CheckBox>(Resource.Id.checkBoxForceMaster);

            AuthButton.Click += AuthButtonClickEvent;
            StorageButton.Click += StorageButtonClickEvent;
        }

        private async void AuthButtonClickEvent(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "Sending data to server!", ToastLength.Short).Show();
            var webApiService = new WebApiService();

            var bearerToken = await webApiService.AuthRequest(LoginFormInput.Text, PasswordFormInput.Text);
            if (!string.IsNullOrEmpty(bearerToken))
            {
                if(ForceMasterCheckBox.Checked)
                {
                    if(await webApiService.PostMasterAccessPoints(AccesspointContainer, bearerToken))
                    {
                        Toast.MakeText(this, "Data sent successfully!", ToastLength.Short).Show();  
                    }
                    else
                    {
                        Toast.MakeText(this, "The data transfer has failed!", ToastLength.Short).Show(); 
                    }
                }
                else
                {
                    if (await webApiService.PostQueueAccessPoints(AccesspointContainer, bearerToken))
                    {
                        Toast.MakeText(this, "Data sent successfully!", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "The data transfer has failed!", ToastLength.Short).Show();
                    }
                }
            }
            else
            {
                Toast.MakeText(this, "Authentication failed!", ToastLength.Short).Show();
            }  
        }

        private async void StorageButtonClickEvent(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "Saving data to local storage!", ToastLength.Short).Show();
            var storageService = new StorageService();
            await storageService.SaveLocalFileToJson(AccesspointContainer);
            Toast.MakeText(this, "Data saved successfully!", ToastLength.Short).Show();
        }
    }
}