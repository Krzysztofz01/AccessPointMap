using AccessPointMapApp.Models;
using AccessPointMapApp.Services;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace AccessPointMapApp
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : AppCompatActivity
	{
		private Button ButtonScan;
		private Button ButtonUpload;
		private Button ButtonFaq;

		private WifiManager WifiManagerObject;
		private List<Accesspoint> AccesspointContainer;
		private Dictionary<string, Accesspoint> AccesspointMap;
		private string[] Permissions = new string[]
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

		protected override void OnCreate(Bundle savedInstanceState)
		{
			Window.SetFlags(Android.Views.WindowManagerFlags.KeepScreenOn, Android.Views.WindowManagerFlags.KeepScreenOn);

			base.OnCreate(savedInstanceState);
			Platform.Init(this, savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			ActivityCompat.RequestPermissions(this, Permissions, 0);
			WifiManagerObject = (WifiManager)GetSystemService(WifiService);
			AccesspointContainer = new List<Accesspoint>();
			AccesspointMap = new Dictionary<string, Accesspoint>();

			if(!WifiManagerObject.IsWifiEnabled)
			{
				Toast.MakeText(this, "Wifi is disabled!", ToastLength.Long).Show();
				WifiManagerObject.SetWifiEnabled(true);
			}

			ButtonScan = FindViewById<Button>(Resource.Id.scanButton);
			ButtonScan.Text = "Start scan";
			ButtonScan.SetTextColor(new Android.Graphics.Color(103, 244, 148));
			ButtonScan.SetBackgroundColor(new Android.Graphics.Color(76, 82, 103));

			ButtonUpload = FindViewById<Button>(Resource.Id.uploadButton);
			ButtonUpload.Text = "Save data";
			ButtonUpload.Enabled = false;
			ButtonUpload.SetTextColor(new Android.Graphics.Color(103, 244, 148));
			ButtonUpload.SetBackgroundColor(new Android.Graphics.Color(76, 82, 103));

			ButtonFaq = FindViewById<Button>(Resource.Id.faqButton);
			ButtonFaq.Text = "Before you start";
			ButtonFaq.SetTextColor(new Android.Graphics.Color(103, 244, 148));
			ButtonFaq.SetBackgroundColor(new Android.Graphics.Color(76, 82, 103));

			ButtonScan.Click += ButtonScanClickEvent;
			ButtonUpload.Click += ButtonUploadClickEvent;
			ButtonFaq.Click += ButtonFaqClickEvent;
		}

		private async void ButtonScanClickEvent(object sender, System.EventArgs e)
		{
			if(ButtonScan.Text == "Start scan")
			{
				ButtonScan.Text = "Stop scan";
				while(ButtonScan.Text == "Stop scan")
				{
					#pragma warning disable CS0612
					//await StartScan();
					await StartScanV2();
					#pragma warning restore CS0612
				}
			}
			else
			{
				ButtonScan.Text = "Start scan";
				if (AccesspointContainer.Count > 0) ButtonUpload.Enabled = true;
			}
		}

		private void ButtonFaqClickEvent(object sender, System.EventArgs e)
		{
			var dialog = new Android.App.AlertDialog.Builder(this);
			var faq = dialog.Create();
			faq.SetTitle("Before you start!");
			faq.SetMessage($@"1. Android application in KitKat version.
2. Turn on the wifi and gps service on your device.
3. Remember to move slowly during the scan, walking is recommended, but cycling is also involved. Driving the car gives incorrect readings for speed reasons.
4. In order to transfer data, you must provide your authentication data and make sure that the 'force master' option is DESIGNED. Remember to check if you are connected to the internet after the scanning process (Wifi or transfer data).
5. It is recommended to disable transfer data before starting the scan.");
			faq.SetButton("OK", (c, ev) => { });
			faq.Show();
		}

		private void ButtonUploadClickEvent(object sender, System.EventArgs e)
		{
			var uploadActivity = new Intent(this, typeof(UploadActivity));
			var serializationService = new SerializationService();

			//uploadActivity.PutExtra("AccessPoints", serializationService.SerializeAccessPointContainer(AccesspointContainer));
			var accesspointMapList = AccesspointMap.Select(x => x.Value).ToList();
			uploadActivity.PutExtra("AccessPoints", serializationService.SerializeAccessPointContainer(accesspointMapList));

			StartActivity(uploadActivity);
		}

		[System.Obsolete]
		private async Task StartScan()
		{
			IList<ScanResult> scanResults = null;
			Location currentLocation = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
			WifiManagerObject.StartScan();
			scanResults = WifiManagerObject.ScanResults;

			foreach(var result in scanResults)
			{
				var knownAccessPoint = AccesspointContainer.Where(element => element.Bssid == result.Bssid).FirstOrDefault();
				if (knownAccessPoint != null)
				{
					if(result.Level > knownAccessPoint.HighSignalLevel)
					{
						knownAccessPoint.HighSignalLevel = result.Level;
						knownAccessPoint.HighLatitude = currentLocation.Latitude;
						knownAccessPoint.HighLongitude = currentLocation.Longitude;
					}

					if(result.Level < knownAccessPoint.LowSignalLevel)
					{
						knownAccessPoint.LowSignalLevel = result.Level;
						knownAccessPoint.LowLatitude = currentLocation.Latitude;
						knownAccessPoint.LowLongitude = currentLocation.Longitude;
					}
				}
				else
				{
					AccesspointContainer.Add(new Accesspoint()
					{
						Bssid = result.Bssid,
						Ssid = (result.Ssid == string.Empty) ? "Hidden network" : result.Ssid,
						Frequency = result.Frequency,
						HighSignalLevel = result.Level,
						HighLongitude = currentLocation.Longitude,
						HighLatitude = currentLocation.Latitude,
						LowSignalLevel = result.Level,
						LowLongitude = currentLocation.Longitude,
						LowLatitude = currentLocation.Latitude,
						SecurityDataRaw = result.Capabilities,
						PostedBy = null
					});
				}
			}
		}

        [System.Obsolete]
        private async Task StartScanV2()
        {
			IList<ScanResult> scanResults = null;
			Location currentLocation = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));
			WifiManagerObject.StartScan();
			scanResults = WifiManagerObject.ScanResults;

			foreach(var result in scanResults)
            {
				try
                {
					AccesspointMap.Add(result.Bssid, new Accesspoint()
					{
						Bssid = result.Bssid,
						Ssid = (result.Ssid == string.Empty) ? "Hidden network" : result.Ssid,
						Frequency = result.Frequency,
						HighSignalLevel = result.Level,
						HighLongitude = currentLocation.Longitude,
						HighLatitude = currentLocation.Latitude,
						LowSignalLevel = result.Level,
						LowLongitude = currentLocation.Longitude,
						LowLatitude = currentLocation.Latitude,
						SecurityDataRaw = result.Capabilities,
						PostedBy = null
					});
                }
				catch(ArgumentException)
                {
					var knownAccessPoint = AccesspointMap[result.Bssid];
					bool changes = false;
					
					if (result.Level > knownAccessPoint.HighSignalLevel)
					{
						knownAccessPoint.HighSignalLevel = result.Level;
						knownAccessPoint.HighLatitude = currentLocation.Latitude;
						knownAccessPoint.HighLongitude = currentLocation.Longitude;
						changes = true;
					}

					if (result.Level < knownAccessPoint.LowSignalLevel)
					{
						knownAccessPoint.LowSignalLevel = result.Level;
						knownAccessPoint.LowLatitude = currentLocation.Latitude;
						knownAccessPoint.LowLongitude = currentLocation.Longitude;
						changes = true;
					}

					if(changes)
                    {
						AccesspointMap[result.Bssid] = knownAccessPoint;
					}
				}
            }
		}


		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
