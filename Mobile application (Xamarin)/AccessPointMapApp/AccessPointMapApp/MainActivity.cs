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

		private WifiManager WifiManagerObject;
		private List<Accesspoint> AccesspointContainer;
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
			Android.Manifest.Permission.WakeLock
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

			if(!WifiManagerObject.IsWifiEnabled)
			{
				Toast.MakeText(this, "Wifi is disabled!", ToastLength.Long).Show();
				WifiManagerObject.SetWifiEnabled(true);
			}

			ButtonScan = FindViewById<Button>(Resource.Id.scanButton);
			ButtonScan.Text = "Start scan";
			ButtonUpload = FindViewById<Button>(Resource.Id.uploadButton);
			ButtonUpload.Text = "Stop scan";
			ButtonUpload.Enabled = false;

			ButtonScan.Click += ButtonScanClickEvent;
			ButtonUpload.Click += ButtonUploadClickEvent;
		}

		private async void ButtonScanClickEvent(object sender, System.EventArgs e)
		{
			if(ButtonScan.Text == "Start scan")
			{
				ButtonScan.Text = "Stop scan";
				while(ButtonScan.Text == "Stop scan")
				{
					#pragma warning disable CS0612
					await StartScan();
					#pragma warning restore CS0612
				}
			}
			else
			{
				ButtonScan.Text = "Start scan";
				if (AccesspointContainer.Count > 0) ButtonUpload.Enabled = true;
			}
		}

		private void ButtonUploadClickEvent(object sender, System.EventArgs e)
		{
			var uploadActivity = new Intent(this, typeof(UploadActivity));
			var serializationService = new SerializationService();
			uploadActivity.PutExtra("AccessPoints", serializationService.SerializeAccessPointContainer(AccesspointContainer));
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
						Ssid = result.Ssid,
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
	
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}
