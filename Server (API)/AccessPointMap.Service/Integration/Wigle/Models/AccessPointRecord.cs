using CsvHelper.Configuration.Attributes;
using System;

namespace AccessPointMap.Service.Integration.Wigle.Models
{
    public class AccessPointRecord
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
    }
}
