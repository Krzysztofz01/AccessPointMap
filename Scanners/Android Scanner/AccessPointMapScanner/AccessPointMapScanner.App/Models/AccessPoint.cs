namespace AccessPointMapScanner.App.Models
{
    public class AccessPoint
    {
        public string Bssid { get; set; }
        public string Ssid { get; set; }
        public double Frequency { get; set; }
        public int MaxSignalLevel { get; set; }
        public double MaxSignalLongitude { get; set; }
        public double MaxSignalLatitude { get; set; }
        public int MinSignalLevel { get; set; }
        public double MinSignalLongitude { get; set; }
        public double MinSignalLatitude { get; set; }
        public string FullSecurityData { get; set; }
    }
}
