namespace AccessPointMapApp.Models
{
    public class Accesspoint
    {
        public string Bssid { get; set; }
        public string Ssid { get; set; }
        public int Frequency { get; set; }
        public int HighSignalLevel { get; set; }  
        public double HighLongitude { get; set; }
        public double HighLatitude { get; set; }
        public int LowSignalLevel { get; set; }
        public double LowLongitude { get; set; }
        public double LowLatitude { get; set; }
        public string SecurityDataRaw { get; set; }
        public string PostedBy { get; set; }
    }
}