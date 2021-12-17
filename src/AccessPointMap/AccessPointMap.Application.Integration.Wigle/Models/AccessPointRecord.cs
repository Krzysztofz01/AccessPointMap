using CsvHelper.Configuration.Attributes;
using System;

namespace AccessPointMap.Application.Integration.Wigle.Models
{
    class AccessPointRecord
    {
        [Index(0)]
        public string Mac { get; set; }

        [Index(1)]
        public string Ssid { get; set; }

        [Index(2)]
        public string AuthMode { get; set; }

        [Index(3)]
        public DateTime FirstSeen { get; set; }

        [Index(4)]
        public int Channel { get; set; }

        [Index(5)]
        public int Rssi { get; set; }

        [Index(6)]
        public double Latitude { get; set; }

        [Index(7)]
        public double Longitude { get; set; }

        [Index(8)]
        public double Altituded { get; set; }

        [Index(9)]
        public double Accuracy { get; set; }

        [Index(10)]
        public string Type { get; set; }


        [Ignore]
        private int _lowSignalLevel;
        [Ignore]
        public int LowSignalLevel
        {
            get => (_lowSignalLevel == default) ? Rssi : _lowSignalLevel;
            set => _lowSignalLevel = value;
        }

        [Ignore]
        private double _lowLatitude;
        [Ignore]
        public double LowLatitude
        {
            get => (_lowLatitude == default) ? Latitude : _lowLatitude;
            set => _lowLatitude = value;
        }

        [Ignore]
        private double _lowLongitude;
        [Ignore]
        public double LowLongitude
        {
            get => (_lowLongitude == default) ? Longitude : _lowLongitude;
            set => _lowLongitude = value;
        }
    }
}
