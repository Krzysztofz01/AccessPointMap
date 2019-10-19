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

        public AccessPoint(string bssid, string ssid, int freq, int signalLevel, double latitude, double longitude)
        {
            this.bssid = bssid;
            this.ssid = ssid;
            this.freq = freq;
            this.signalLevel = signalLevel;
            this.latitude = latitude;
            this.longitude = longitude;
        }
    }
}