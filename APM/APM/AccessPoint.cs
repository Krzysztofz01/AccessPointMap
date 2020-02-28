using System.Collections.Generic;

namespace APM
{
    class AccessPoint
    {
        public static List<AccessPoint> AccessPointContainer = new List<AccessPoint>();
        
        public string bssid { get; set; }
        public string ssid { get; set; }
        public int freq { get; set; }
        public int signalLevel { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int lowSignalLevel { get; set; }
        public double lowLatitude { get; set; }
        public double lowLongitude { get; set; }
        public string security { get; set; }

        public AccessPoint(string bssid, string ssid, int freq, int signalLevel, double latitude, double longitude, int lowSignalLevel, double lowLatitude, double lowLongitude, string security)
        {
            this.bssid = bssid;
            this.ssid = ssid;
            this.freq = freq;
            this.signalLevel = signalLevel;
            this.latitude = latitude;
            this.longitude = longitude;
            this.lowSignalLevel = lowSignalLevel;
            this.lowLatitude = lowLatitude;
            this.lowLongitude = lowLongitude;
            this.security = security;
        }

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
    }
}