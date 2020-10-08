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
using Android.Content;

namespace APM
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        //Init GUI elements objects
        private RadioGroup dataStoreMethod;
        private RadioGroup dataScanMethod;
        private RadioGroup geolocationAccuracy;
        private Button buttonScan;
        private Button buttonUpload;

        //Init the WifiManager object
        private WifiManager wifiManager;

        //Init the Geolocation handler variable
        private GeolocationRequest locationRequest;

        //Define permissions
        private string[] permissions = new string[]
        {
            Android.Manifest.Permission.AccessFineLocation,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.WakeLock
        }; 
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Prevent app from sleeping
            Window.SetFlags(Android.Views.WindowManagerFlags.KeepScreenOn, Android.Views.WindowManagerFlags.KeepScreenOn);

            //Init the activity and view
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            //Permission check (mainly for the fine location)
            ActivityCompat.RequestPermissions(this, permissions, 0);

            //Set the wifiManager
            wifiManager = (WifiManager)GetSystemService(WifiService);

            //Check if the WiFi is enabled
            if (!wifiManager.IsWifiEnabled)
            {
                Toast.MakeText(this, "Wifi is disabled!", ToastLength.Long).Show();
                wifiManager.SetWifiEnabled(true);
            }

            //Assign GUI elemets to objects
            dataStoreMethod = FindViewById<RadioGroup>(Resource.Id.radioGroupStoreMethod);
            dataScanMethod = FindViewById<RadioGroup>(Resource.Id.radioGroupScanMethod);
            geolocationAccuracy = FindViewById<RadioGroup>(Resource.Id.radioGroupAccuracyMethod);
            buttonScan = FindViewById<Button>(Resource.Id.buttonStartScan);
            buttonUpload = FindViewById<Button>(Resource.Id.buttonStartUpload);

            //Assign events to Button elements
            buttonScan.Click += buttonScanClickEvent;
            buttonUpload.Click += buttonUploadClickEvent;
        }

        private async void buttonScanClickEvent(object sender, System.EventArgs e)
        {
            if (buttonScan.Text == Resources.GetString(Resource.String.buttonScan))
            {
                buttonScan.Text = Resources.GetString(Resource.String.buttonStop);
                
                //Set the geolocation accuracy
                switch(geolocationAccuracy.CheckedRadioButtonId)
                {
                    case Resource.Id.accuracyBest: locationRequest = new GeolocationRequest(GeolocationAccuracy.Best); break;
                    case Resource.Id.accuracyHigh: locationRequest = new GeolocationRequest(GeolocationAccuracy.High); break;
                    case Resource.Id.accuracyMedium: locationRequest = new GeolocationRequest(GeolocationAccuracy.Medium); break;
                    default: locationRequest = new GeolocationRequest(GeolocationAccuracy.Best); break;
                }

                if (dataScanMethod.CheckedRadioButtonId == Resource.Id.lightMethod)
                {
                    while (buttonScan.Text == Resources.GetString(Resource.String.buttonStop))
                    {
                        await lightScanMethod();                       
                    }
                }
                else if(dataScanMethod.CheckedRadioButtonId == Resource.Id.hardMethod)
                {
                    while (buttonScan.Text == Resources.GetString(Resource.String.buttonStop))
                    {
                        await hardScanMethod();
                    }
                }
            }
            else
            {
                buttonScan.Text = Resources.GetString(Resource.String.buttonScan);
            }
        }

        private async void buttonUploadClickEvent(object sender, System.EventArgs e)
        {
            /*switch(dataStoreMethod.CheckedRadioButtonId)
            {
                case Resource.Id.localFile: {
                    //Use the Local class method to save data to a .JSON file
                    Local.saveToDeviceLight(AccessPoint.AccessPointContainer);
                    Toast.MakeText(this, "Data saved to a local file!", ToastLength.Long).Show();
                    } break;
                case Resource.Id.apiCall: {
                    //Create a new ApiHelper object to send JSON data to REST API
                    ApiHelper api = new ApiHelper();


                    //TODO: LOGIN AND PASSWORD FORM HERE
                    string login = "";
                    string password = "";

                    if(await api.send(AccessPoint.AccessPointContainer, login, password))
                    {
                        Toast.MakeText(this, "Data successful posted!", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "Some errors occured while posting data!", ToastLength.Long).Show();
                    }                 
                    } break;
                default: break;
            }
            */
            buttonScan.Text = Resources.GetString(Resource.String.buttonScan);
            StartActivity(new Intent(this, typeof(UploadActivity)));
        }


        private async Task lightScanMethod()
        {
            //Init a helper array to store scan result temporary and a boolean to check if the currently checked AP is already known
            IList<ScanResult> scanResults = null;
            bool alreadyKnown = false;

            //Get the current location
            Location currentLocation = await Geolocation.GetLocationAsync(locationRequest);

            //Scan for near wireless networks, this method is deprecated (turning off compiler warning)
            #pragma warning disable 618
            wifiManager.StartScan();
            #pragma warning restore 618

            //Assign the results to the temp container
            scanResults = wifiManager.ScanResults;

            //Compare and perm store to static container
            if(AccessPoint.AccessPointContainer.Count > 0)
            {
                foreach (ScanResult element in scanResults)
                {
                    alreadyKnown = false;
                    foreach (AccessPoint known in AccessPoint.AccessPointContainer)
                    {
                        if(element.Bssid == known.bssid)
                        {
                            alreadyKnown = true;

                            if(element.Level > known.highSignalLevel)
                            {
                                known.highSignalLevel = element.Level;
                                known.highLatitude = currentLocation.Latitude;
                                known.highLongitude = currentLocation.Longitude;
                            }
                            else if(element.Level < known.lowSignalLevel)
                            {
                                known.lowSignalLevel = element.Level;
                                known.lowLatitude = currentLocation.Latitude;
                                known.lowLongitude = currentLocation.Longitude;
                            }

                            break;
                        }
                    }

                    if(!alreadyKnown)
                    {
                        AccessPoint.AccessPointContainer.Add(new AccessPoint(
                            element.Bssid,
                            element.Ssid,
                            element.Frequency,
                            element.Level,
                            currentLocation.Latitude,
                            currentLocation.Longitude,
                            element.Level,
                            currentLocation.Latitude,
                            currentLocation.Longitude,
                            element.Capabilities));
                    }
                }
            }
            else
            {
                AccessPoint.AccessPointContainer.Add(new AccessPoint(
                            scanResults[0].Bssid,
                            scanResults[0].Ssid,
                            scanResults[0].Frequency,
                            scanResults[0].Level,
                            currentLocation.Latitude,
                            currentLocation.Longitude,
                            scanResults[0].Level,
                            currentLocation.Latitude,
                            currentLocation.Longitude,
                            scanResults[0].Capabilities));
            }  
        }

        private async Task hardScanMethod()
        {
            //Init a helper array to store scan result temporary
            IList<ScanResult> scanResults = null;
       
            //Get the current location
            Location currentLocation = await Geolocation.GetLocationAsync(locationRequest);

            //Scan for near wireless networks, this method is deprecated (turning off compiler warning)
            #pragma warning disable 618
            wifiManager.StartScan();
            #pragma warning restore 618

            //Assign the results to the temp container
            scanResults = wifiManager.ScanResults;

            foreach (ScanResult element in scanResults)
            {
                Local.saveToDeviceHard(new AccessPoint(
                    element.Bssid,
                    element.Ssid,
                    element.Frequency,
                    element.Level,
                    currentLocation.Latitude,
                    currentLocation.Longitude,
                    element.Capabilities));
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}