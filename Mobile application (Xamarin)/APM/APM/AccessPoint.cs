using System.Collections.Generic;

namespace APM
{
    class AccessPoint
    {
        public static List<AccessPoint> AccessPointContainer = new List<AccessPoint>();
        
        public string bssid { get; set; }
        public string ssid { get; set; }
        public int frequency { get; set; }
        public int highSignalLevel { get; set; }
        public double highLatitude { get; set; }
        public double highLongitude { get; set; }
        public int lowSignalLevel { get; set; }
        public double lowLatitude { get; set; }
        public double lowLongitude { get; set; }
        public string securityData { get; set; }

        public AccessPoint(string bssid, string ssid, int freq, int signalLevel, double latitude, double longitude, int lowSignalLevel, double lowLatitude, double lowLongitude, string security)
        {
            this.bssid = bssid;
            this.ssid = ssid;
            this.frequency = freq;
            this.highSignalLevel = signalLevel;
            this.highLatitude = latitude;
            this.highLongitude = longitude;
            this.lowSignalLevel = lowSignalLevel;
            this.lowLatitude = lowLatitude;
            this.lowLongitude = lowLongitude;
            this.securityData = security;
        }

        public AccessPoint(string bssid, string ssid, int freq, int signalLevel, double latitude, double longitude, string security)
        {
            this.bssid = bssid;
            this.ssid = ssid;
            this.frequency = freq;
            this.highSignalLevel = signalLevel;
            this.highLatitude = latitude;
            this.highLongitude = longitude;
            this.securityData = security;
        }
    }
}