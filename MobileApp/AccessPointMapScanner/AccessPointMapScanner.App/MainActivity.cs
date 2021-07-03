using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views;
using Android.Content.PM;
using Android.Net.Wifi;
using System.Collections.Generic;
using Android.Support.V4.App;
using System.Threading.Tasks;
using System;
using Xamarin.Essentials;
using AccessPointMapScanner.App.Models;
using Android.Content;
using AccessPointMapScanner.App.Utilities;

namespace AccessPointMapScanner.App
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        //UI Components
        private Button scanButton;
        private Button uploadButton;
        private Button saveButton;
        private Button websiteButton;
        private TextView countLabel;

        //Scan utilites
        private WifiManager wifiManager;
        private IDictionary<string, AccessPoint> accessPointDict;

        //Android permissions
        private readonly string[] permissions = new string[]
        {
            Android.Manifest.Permission.AccessNetworkState,
            Android.Manifest.Permission.AccessCoarseLocation,
            Android.Manifest.Permission.AccessFineLocation,
            Android.Manifest.Permission.AccessWifiState,
            Android.Manifest.Permission.ChangeWifiState,
            Android.Manifest.Permission.ChangeNetworkState,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.WakeLock,
            Android.Manifest.Permission.ReadPhoneState
        };

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            //Keep screen on rule and remove navbar
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            SupportActionBar.Hide();

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Permission and initialization
            ActivityCompat.RequestPermissions(this, permissions, 0);
            wifiManager = (WifiManager)GetSystemService(WifiService);
            accessPointDict = new Dictionary<string, AccessPoint>();

            //UI Component initialization
            scanButton = FindViewById<Button>(Resource.Id.scanButton);
            scanButton.Enabled = true;
            scanButton.Click += ScanButtonClickEvent;

            uploadButton = FindViewById<Button>(Resource.Id.uploadButton);
            uploadButton.Enabled = false;
            uploadButton.Click += UploadButtonClickEvent;

            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Enabled = false;
            saveButton.Click += SaveButtonClickEvent;

            websiteButton = FindViewById<Button>(Resource.Id.websiteButton);
            websiteButton.Enabled = true;
            websiteButton.Click += WebsiteButtonClickEvent;

            countLabel = FindViewById<TextView>(Resource.Id.countLabel);
            countLabel.Text = string.Empty;
        }

        private async Task<bool> CheckServices()
        {
            //Check if WiFi is enabled on devive
            if (!wifiManager.IsWifiEnabled)
            {
                Toast.MakeText(this, "Wifi is disabled! Turning on...", ToastLength.Long).Show();
                wifiManager.SetWifiEnabled(true);
            }

            //Check if Locations is enabled on device
            try
            {
                await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
            }
            catch (Exception)
            {
                Toast.MakeText(this, "GPS is disabled! Enable the GPS service and than start the app!", ToastLength.Long).Show();
                return false;
            }
            return true;
        }

        //Event methods
        private void WebsiteButtonClickEvent(object sender, System.EventArgs e)
        {
            var websiteActivity = new Intent(this, typeof(WebsiteActivity));
            StartActivity(websiteActivity);
        }

        private async void SaveButtonClickEvent(object sender, System.EventArgs e)
        {
            Toast.MakeText(this, "Saving results to device storage...", ToastLength.Short).Show();

            try
            {
                await Utilities.StorageService.SaveResultsToJsonLocal(accessPointDict);
            }
            catch(Exception)
            {
                Toast.MakeText(this, "Results saving failed!", ToastLength.Long).Show();
                return;
            }

            Toast.MakeText(this, "Results stored successful!", ToastLength.Short).Show();
        }

        private void UploadButtonClickEvent(object sender, System.EventArgs e)
        {
            var uploadActivity = new Intent(this, typeof(UploadActivity));
            var accessPoints = SerializationService.SerializeAccessPoints(accessPointDict);
            uploadActivity.PutExtra("accesspoints", accessPoints);
            StartActivity(uploadActivity);
        }

        private async void ScanButtonClickEvent(object sender, System.EventArgs e)
        {
            if(scanButton.Text == "SCAN")
            {
                if (!await CheckServices()) return;

                websiteButton.Enabled = false;
                scanButton.Text = "STOP";

                while(scanButton.Text == "STOP")
                {
                    #pragma warning disable CS0612
                    await StartScanV1();
                    #pragma warning restore CS0612

                    countLabel.Text = GetCountText();
                }
            }
            else
            {
                scanButton.Text = "SCAN";
                uploadButton.Enabled = true;
                saveButton.Enabled = true;
                websiteButton.Enabled = true;
            }
        }

        //Utilites methods
        [Obsolete]
        private async Task StartScanV1()
        {
            wifiManager.StartScan();
            var scanResults = wifiManager.ScanResults;
            var locationResults = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

            foreach (var result in scanResults)
            {
                if (accessPointDict.ContainsKey(result.Bssid))
                {
                    if (result.Level > accessPointDict[result.Bssid].MaxSignalLevel)
                    {
                        accessPointDict[result.Bssid].MaxSignalLevel = result.Level;
                        accessPointDict[result.Bssid].MaxSignalLatitude = locationResults.Latitude;
                        accessPointDict[result.Bssid].MaxSignalLongitude = locationResults.Longitude;
                    }

                    if (result.Level < accessPointDict[result.Bssid].MinSignalLevel)
                    {
                        accessPointDict[result.Bssid].MinSignalLevel = result.Level;
                        accessPointDict[result.Bssid].MinSignalLatitude = locationResults.Latitude;
                        accessPointDict[result.Bssid].MinSignalLongitude = locationResults.Longitude;
                    }
                }
                else
                {
                    accessPointDict.Add(result.Bssid, new AccessPoint
                    {
                        Bssid = result.Bssid,
                        Ssid = result.Ssid,
                        Frequency = result.Frequency,
                        MaxSignalLevel = result.Level,
                        MaxSignalLatitude = locationResults.Latitude,
                        MaxSignalLongitude = locationResults.Longitude,
                        MinSignalLevel = result.Level,
                        MinSignalLatitude = locationResults.Latitude,
                        MinSignalLongitude = locationResults.Longitude,
                        FullSecurityData = result.Capabilities
                    }); ;
                }
            }
        }

        private string GetCountText()
        {
            return $"Access points collected: { accessPointDict.Count }";
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}