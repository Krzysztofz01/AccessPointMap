using System;

namespace ms_accesspointmap_api.Models
{
    public partial class GuestAccesspoints
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
        public string SecurityDataRaw { get; set; }
        public string DeviceType { get; set; }
        public string PostedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
