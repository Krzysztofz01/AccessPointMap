using System.Collections.Generic;

namespace APM
{
    class AccessPoint
    {
        public static List<AccessPoint> AccessPointContainer = new List<AccessPoint>();
        public static List<AccessPoint> AccessPointKnown = new List<AccessPoint>();

        public string bssid { get; set; }
        public string ssid { get; set; }
        public int freq { get; set; }
        public int signalLevel { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public string security { get; set; }

        public AccessPoint(string bssid, string ssid, int freq, int signalLevel, double latitude, double longitude, string security)
        {
            this.bssid = bssid;
            this.ssid = ssid;
            this.freq = freq;
            this.signalLevel = signalLevel;
            this.latitude = latitude;
            this.longitude = longitude;
            this.security = security;
        }

        public AccessPoint(string bssid, int signalLevel)
        {
            this.bssid = bssid;
            this.signalLevel = signalLevel;

            this.ssid = null;
            this.freq = 0;
            this.latitude = 0;
            this.longitude = 0;
            this.security = null;
        }

        public static string securityType(string capabilities)
        {
            if (capabilities.Contains("WPA2"))
            {
                return "WPA";
            }
            else if (capabilities.Contains("WPA"))
            {
                return "WPA";
            }
            else if (capabilities.Contains("WEP"))
            {
                return "WEP";
            }
            else if (capabilities.Contains("WPS"))
            {
                return "WPS";
            }
            else
            {
                return "none";
            }
        }
    }
}