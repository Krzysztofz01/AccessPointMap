namespace AccessPointMap.Domain
{
    public class AccessPoint : BaseEntity
    {
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
        public long? UserAddedId { get; set; }
        public virtual User UserAdded { get; set; }
        public long? UserModifiedId { get; set; }
        public virtual User UserModified { get; set; }
    }
}
