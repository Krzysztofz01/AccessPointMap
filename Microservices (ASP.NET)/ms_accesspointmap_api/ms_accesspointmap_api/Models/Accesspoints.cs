using System;
using System.Collections.Generic;

namespace ms_accesspointmap_api.Models
{
    public partial class Accesspoints
    {
        public int Id { get; set; }
        public string Bssid { get; set; }
        public string Ssid { get; set; }
        public int Frequency { get; set; }
        public int HighSignalLevel { get; set; }
        public double HighLongitude { get; set; }
        public double HighLatitude { get; set; }
        public int LowSignalLevel { get; set; }
        public double LowLongitude { get; set; }
        public double LowLatitude { get; set; }
        public double SignalRadius { get; set; }
        public double SignalArea { get; set; }
        public string SecurityData { get; set; }
        public string Brand { get; set; }
        public string DeviceType { get; set; }
        public bool? Display { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
