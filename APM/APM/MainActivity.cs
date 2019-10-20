using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Net.Wifi;
using System.Collections.Generic;
using Android.Support.V4.App;
using Xamarin.Essentials;

namespace APM
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button scanButton;
        private Button uploadButton;
        private TextView accessPointCount;

        private GeolocationRequest request;
        private WifiManager wifiManager;

        public IList<ScanResult> scanResultArray;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            scanButton = FindViewById<Button>(Resource.Id.startScan);
            scanButton.Click += ScanButton_Click;

            uploadButton = FindViewById<Button>(Resource.Id.uploadData);
            uploadButton.Click += UploadButton_Click;

            accessPointCount = FindViewById<TextView>(Resource.Id.accessPointCount);
            accessPointCount.Text = "0";

            wifiManager = (WifiManager)GetSystemService(WifiService);



            //Enable wifi if disabled
            if (!wifiManager.IsWifiEnabled)
            {
                Toast.MakeText(this, "Wifi is disabled!", ToastLength.Long).Show();
                wifiManager.SetWifiEnabled(true);
            }

            //Ask for permission (location)
            ActivityCompat.RequestPermissions(this, new string[] { Android.Manifest.Permission.AccessFineLocation }, 0);

        }

        private void UploadButton_Click(object sender, System.EventArgs e)
        {
            ApiHelper api = new ApiHelper();
            api.send(AccessPoint.AccessPointContainer);
        }

      
        private async void ScanButton_Click(object sender, System.EventArgs e)
        {
            startScan();
            accessPointCount.Text = AccessPoint.AccessPointContainer.Count.ToString();
        }

        
        private async void startScan()
        {
            //get current location
            request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            wifiManager.StartScan();
            scanResultArray = wifiManager.ScanResults;

            bool exist = false;
            for(int i=0; i<scanResultArray.Count; i++)
            {
                exist = false;
                for(int l=0; l<AccessPoint.AccessPointContainer.Count; l++)
                {
                    if(scanResultArray[i].Bssid == AccessPoint.AccessPointContainer[l].bssid)
                    {
                        exist = true;
                        AccessPoint.AccessPointContainer[l].latitude = location.Latitude;
                        AccessPoint.AccessPointContainer[l].longitude = location.Longitude;
                    }
                }

                if(!exist)
                {
                    AccessPoint.AccessPointContainer.Add(new AccessPoint(
                        scanResultArray[i].Bssid,
                        scanResultArray[i].Ssid,
                        scanResultArray[i].Frequency,
                        scanResultArray[i].Level,
                        location.Latitude,
                        location.Longitude));
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}