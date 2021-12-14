using CsvHelper.Configuration.Attributes;
using System;

namespace AccessPointMap.Application.Integration.Aircrackng.Models
{
    public class AccessPointRecord
    {
        [Index(0)]
        public DateTime LocalTimestamp { get; set; }

        [Index(1)]
        public DateTime GpsTimestamp { get; set; }

        [Index(2)]
        public string Ssid { get; set; }

        [Index(3)]
        public string Bssid { get; set; }

        [Index(4)]
        public int Power { get; set; }

        [Index(5)]
        public string Security { get; set; }

        [Index(6)]
        public double Latitude { get; set; }

        [Index(7)]
        public double Longitude { get; set; }

        [Index(8)]
        public double LatitudeError { get; set; }

        [Index(9)]
        public double LongitudeError { get; set; }

        [Index(10)]
        public string Type { get; set; }

        public AccessPointFull ToFull()
        {
            return new AccessPointFull
            {
                LocalTimestamp = LocalTimestamp,
                GpsTimestamp = GpsTimestamp,
                Ssid = Ssid,
                Bssid = Bssid,
                Security = Security,
                LowPower = Power,
                LowLatitude = Latitude,
                LowLongitude = Longitude,
                HighPower = Power,
                HighLatitude = Latitude,
                HighLongitude = Longitude,
                LatitudeError = LatitudeError,
                LongitudeError = LongitudeError,
                Type = Type
            };
        }
    }
}
