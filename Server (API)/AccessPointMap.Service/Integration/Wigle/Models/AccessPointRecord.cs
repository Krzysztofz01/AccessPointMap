using CsvHelper.Configuration.Attributes;

namespace AccessPointMap.Service.Integration.Wigle.Models
{
    public class AccessPointRecord
    {
        [Index(0)]
        public string Bssid { get; set; }
        
        [Index(1)]
        public string Ssid { get; set; }
        
        [Index(2)]
        public string FullSecurityData { get; set; }
        
        [Index(4)]
        public int Channel { get; set; }
        
        [Index(5)]
        public int MaxSignalLevel { get; set; }
        
        [Index(6)]
        public double MaxSignalLatitude { get; set; }
        
        [Index(7)]
        public double MaxSignalLongitude { get; set; }
        
        [Index(8)]
        public double MaxSignalAltitude { get; set; }
        
        [Index(9)]
        public double Accuracy { get; set; }
        
        [Index(10)]
        public string DeviceType { get; set; }
    }
}
