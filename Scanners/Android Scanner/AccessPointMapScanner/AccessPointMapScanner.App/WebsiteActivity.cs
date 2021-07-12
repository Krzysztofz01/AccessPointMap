using AccessPointMapScanner.App.Utilities;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Webkit;

namespace AccessPointMapScanner.App
{
    [Activity(Label = "WebsiteActivity")]
    public class WebsiteActivity : Activity
    {
        private WebView webview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Keep screen on rule and remove navbar
            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_website);

            webview = FindViewById<WebView>(Resource.Id.apmView);
            webview.LoadUrl(WebService.website);
        }
    }
}