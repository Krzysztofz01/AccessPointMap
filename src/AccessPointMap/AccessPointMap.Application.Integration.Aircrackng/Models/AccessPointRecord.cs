using CsvHelper.Configuration.Attributes;
using System;
using System.Text.Json.Serialization;

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

        [Ignore]
        [JsonIgnore]
        private int _lowSignalLevel;
        [Ignore]
        [JsonIgnore]
        public int LowSignalLevel
        {
            get => (_lowSignalLevel == default) ? Power : _lowSignalLevel;
            set => _lowSignalLevel = value;
        }

        [Ignore]
        [JsonIgnore]
        private double _lowLatitude;
        [Ignore]
        [JsonIgnore]
        public double LowLatitude
        {
            get => (_lowLatitude == default) ? Latitude : _lowLatitude;
            set => _lowLatitude = value;
        }

        [Ignore]
        [JsonIgnore]
        private double _lowLongitude;
        [Ignore]
        [JsonIgnore]
        public double LowLongitude
        {
            get => (_lowLongitude == default) ? Longitude : _lowLongitude;
            set => _lowLongitude = value;
        }
    }
}
