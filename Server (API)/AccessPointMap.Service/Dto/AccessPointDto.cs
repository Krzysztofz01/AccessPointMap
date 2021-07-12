using System;

namespace AccessPointMap.Service.Dto
{
    public class AccessPointDto
    {
        public long Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Bssid { get; set; }
        public string Ssid { get; set; }
        public string Fingerprint { get; set; }
        public double Frequency { get; set; }
        public int MaxSignalLevel { get; set; }
        public double MaxSignalLongitude { get; set; }
        public double MaxSignalLatitude { get; set; }
        public int MinSignalLevel { get; set; }
        public double MinSignalLongitude { get; set; }
        public double MinSignalLatitude { get; set; }
        public double SignalRadius { get; set; }
        public double SignalArea { get; set; }
        public string FullSecurityData { get; set; }
        public string SerializedSecurityData { get; set; }
        public string Manufacturer { get; set; }
        public string DeviceType { get; set; }
        public bool MasterGroup { get; set; }
        public bool Display { get; set; }
        public string Note { get; set; }
        public bool IsSecure { get; set; }
        public bool IsHidden { get; set; }
        public long? UserAddedId { get; set; }
        public UserDto UserAdded { get; set; }
        public long? UserModifiedId { get; set; }
        public UserDto UserModified { get; set; }
    }
}
