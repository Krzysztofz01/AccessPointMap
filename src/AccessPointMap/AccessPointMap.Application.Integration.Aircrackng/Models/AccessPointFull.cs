using System;

namespace AccessPointMap.Application.Integration.Aircrackng.Models
{
    public class AccessPointFull
    {
        public DateTime LocalTimestamp { get; set; }

        public DateTime GpsTimestamp { get; set; }

        public string Ssid { get; set; }

        public string Bssid { get; set; }

        public string Security { get; set; }

        public int LowPower { get; set; }

        public double LowLatitude { get; set; }

        public double LowLongitude { get; set; }

        public int HighPower { get; set; }

        public double HighLatitude { get; set; }

        public double HighLongitude { get; set; }

        public double LatitudeError { get; set; }

        public double LongitudeError { get; set; }

        public string Type { get; set; }
    }
}
