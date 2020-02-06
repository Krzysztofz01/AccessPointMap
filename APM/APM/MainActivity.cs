using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Net.Wifi;
using System.Collections.Generic;
using Android.Support.V4.App;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace APM
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button scanButton;
        private Button uploadButton;
        private TextView accessPointCount;
        private TextView scanStatus;
        private TextView uploadStatus;
        private CheckBox saveLocal;
        private CheckBox sendToApi;
        private CheckBox bestAccuracy;
        private CheckBox hardSave;

        private GeolocationRequest request;
        private WifiManager wifiManager;
        private string[] permissions = new string[]
        {
            Android.Manifest.Permission.AccessFineLocation,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ReadExternalStorage
        };

        public IList<ScanResult> scanResultArray;

        private bool scanLoop = false;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            scanButton = FindViewById<Button>(Resource.Id.startScan);
            scanButton.Click += ScanButton_Click;

            uploadButton = FindViewById<Button>(Resource.Id.uploadData);
            uploadButton.Click += UploadButton_Click;

            accessPointCount = FindViewById<TextView>(Resource.Id.accessPointCount);
            accessPointCount.Text = "0";

            scanStatus = FindViewById<TextView>(Resource.Id.scanStatus);
            scanStatus.Text = scanLoop.ToString();

            uploadStatus = FindViewById<TextView>(Resource.Id.uploadStatus);

            saveLocal = FindViewById<CheckBox>(Resource.Id.saveLocal);
            saveLocal.Checked = true;

            sendToApi = FindViewById<CheckBox>(Resource.Id.sendToApi);
            sendToApi.Checked = true;

            bestAccuracy = FindViewById<CheckBox>(Resource.Id.bestAccuracy);
            bestAccuracy.Checked = true;

            hardSave = FindViewById<CheckBox>(Resource.Id.hardSave);
            hardSave.Checked = false;
            

            wifiManager = (WifiManager)GetSystemService(WifiService);

            if (!wifiManager.IsWifiEnabled)
            {
                Toast.MakeText(this, "Wifi is disabled!", ToastLength.Long).Show();
                wifiManager.SetWifiEnabled(true);
            }

            //Permission check
            ActivityCompat.RequestPermissions(this, permissions, 0);

        }

        private async void UploadButton_Click(object sender, System.EventArgs e)
        {
            scanLoop = false;
            if (saveLocal.Checked)
            {
                Local.saveToDeviceLight(AccessPoint.AccessPointContainer);
                Toast.MakeText(this, "Data saved to local storage", ToastLength.Long).Show();
            }

            if (sendToApi.Checked)
            {
                ApiHelper api = new ApiHelper();
       
                //Debug
                System.Diagnostics.Debug.WriteLine("Sending to API");
                await api.send(AccessPoint.AccessPointContainer);
                uploadStatus.Text = "Upload process finished!";
                Toast.MakeText(this, "Data send to API", ToastLength.Long).Show();
            }
        }
     
        private async void ScanButton_Click(object sender, System.EventArgs e)
        {
            if(scanLoop)
            {
                scanLoop = false;
                scanStatus.Text = scanLoop.ToString();
            }
            else
            {
                scanLoop = true;
                scanStatus.Text = scanLoop.ToString();
            }

            while(scanLoop)
            {
                if(!hardSave.Checked)
                {
                    //Debug
                    System.Diagnostics.Debug.WriteLine("Scan..");
                    await startScan();
                    accessPointCount.Text = AccessPoint.AccessPointContainer.Count.ToString();
                }
                else
                {
                    await startHardScan();
                }
                
            }
        }

        private async Task startScan()
        {
            //Get current location
            if(bestAccuracy.Checked)
            {
                request = new GeolocationRequest(GeolocationAccuracy.Best);
            }
            else
            {
                request = new GeolocationRequest(GeolocationAccuracy.High);
            }
            
            var location = await Geolocation.GetLocationAsync(request);

            wifiManager.StartScan();
            scanResultArray = wifiManager.ScanResults;

            bool exist = false;
            for(int i=0; i<scanResultArray.Count; i++)
            {
                //debug
                System.Diagnostics.Debug.WriteLine(scanResultArray[i].Ssid + " " + scanResultArray[i].Level);

                exist = false;
                for(int l=0; l<AccessPoint.AccessPointContainer.Count; l++)
                {
                    if(scanResultArray[i].Bssid == AccessPoint.AccessPointContainer[l].bssid)
                    {
                        exist = true;
                        if(scanResultArray[i].Level > AccessPoint.AccessPointContainer[l].signalLevel)
                        {
                            AccessPoint.AccessPointContainer[l].signalLevel = scanResultArray[i].Level;
                            AccessPoint.AccessPointContainer[l].latitude = location.Latitude;
                            AccessPoint.AccessPointContainer[l].longitude = location.Longitude;
                        }
                        else if(scanResultArray[i].Level < AccessPoint.AccessPointContainer[l].lowSignalLevel)
                        {
                            AccessPoint.AccessPointContainer[l].lowSignalLevel = scanResultArray[i].Level;
                            AccessPoint.AccessPointContainer[l].lowLatitude = location.Latitude;
                            AccessPoint.AccessPointContainer[l].lowLongitude = location.Longitude;
                        }
                        break;
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
                        location.Longitude,
                        scanResultArray[i].Level,
                        location.Latitude,
                        location.Longitude,
                        scanResultArray[i].Capabilities));
                }
            }
        }

        private async Task startHardScan()
        {
            //Get current location
            if (bestAccuracy.Checked)
            {
                request = new GeolocationRequest(GeolocationAccuracy.Best);
            }
            else
            {
                request = new GeolocationRequest(GeolocationAccuracy.High);
            }

            var location = await Geolocation.GetLocationAsync(request);

            wifiManager.StartScan();
            scanResultArray = wifiManager.ScanResults;

            for(int i=0; i<scanResultArray.Count; i++)
            {
                Local.saveToDeviceHard(new AccessPoint(
                scanResultArray[i].Bssid,
                scanResultArray[i].Ssid,
                scanResultArray[i].Frequency,
                scanResultArray[i].Level,
                location.Latitude,
                location.Longitude,
                scanResultArray[i].Capabilities));
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}