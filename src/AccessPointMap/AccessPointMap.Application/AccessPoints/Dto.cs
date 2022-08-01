using System;
using System.Collections.Generic;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Dto
    {
        public class AccessPointSimple
        {
            public Guid Id { get; set; }
            public string Bssid { get; set; }
            public string Ssid { get; set; }
            public double Frequency { get; set; }
            public string DeviceType { get; set; }
            public double HighSignalLatitude { get; set; }
            public double HighSignalLongitude { get; set; }
            public double SignalArea { get; set; }
            public string SecurityStandards { get; set; }
            public string SecurityProtocols { get; set; }
            public bool IsSecure { get; set; }
            public Guid? RunIdentifier { get; set; }
        }

        public class AccessPointDetails
        {
            public Guid Id { get; set; }
            public string Bssid { get; set; }
            public string Manufacturer { get; set; }
            public string Ssid { get; set; }
            public double Frequency { get; set; }
            public string DeviceType { get; set; }
            public Guid ContributorId { get; set; }
            public DateTime CreationTimestamp { get; set; }
            public DateTime VersionTimestamp { get; set; }
            public int LowSignalLevel { get; set; }
            public double LowSignalLatitude { get; set; }
            public double LowSignalLongitude { get; set; }
            public int HighSignalLevel { get; set; }
            public double HighSignalLatitude { get; set; }
            public double HighSignalLongitude { get; set; }
            public double SignalRadius { get; set; }
            public double SignalArea { get; set; }
            public string RawSecurityPayload { get; set; }
            public string SecurityStandards { get; set; }
            public string SecurityProtocols { get; set; }
            public bool IsSecure { get; set; }
            public bool IsPresent { get; set; }
            public Guid? RunIdentifier { get; set; }
            public IEnumerable<AccessPointStampDetails> Stamps { get; set; }
        }

        public class AccessPointDetailsAdministration
        {
            public Guid Id { get; set; }
            public string Bssid { get; set; }
            public string Manufacturer { get; set; }
            public string Ssid { get; set; }
            public double Frequency { get; set; }
            public string DeviceType { get; set; }
            public Guid ContributorId { get; set; }
            public DateTime CreationTimestamp { get; set; }
            public DateTime VersionTimestamp { get; set; }
            public int LowSignalLevel { get; set; }
            public double LowSignalLatitude { get; set; }
            public double LowSignalLongitude { get; set; }
            public int HighSignalLevel { get; set; }
            public double HighSignalLatitude { get; set; }
            public double HighSignalLongitude { get; set; }
            public double SignalRadius { get; set; }
            public double SignalArea { get; set; }
            public string RawSecurityPayload { get; set; }
            public string SecurityStandards { get; set; }
            public string SecurityProtocols { get; set; }
            public bool IsSecure { get; set; }
            public bool IsPresent { get; set; }
            public string Note { get; set; }
            public bool DisplayStatus { get; set; }
            public Guid? RunIdentifier { get; set; }
            public IEnumerable<AccessPointStampDetails> Stamps { get; set; }
            public IEnumerable<AccessPointAdnnotationDetails> Adnnotations { get; set; }
        }

        public class AccessPointStampDetails
        {
            public Guid Id { get; set; }
            public string Ssid { get; set; }
            public double Frequency { get; set; }
            public string DeviceType { get; set; }
            public Guid ContributorId { get; set; }
            public DateTime CreationTimestamp { get; set; }
            public int LowSignalLevel { get; set; }
            public double LowSignalLatitude { get; set; }
            public double LowSignalLongitude { get; set; }
            public int HighSignalLevel { get; set; }
            public double HighSignalLatitude { get; set; }
            public double HighSignalLongitude { get; set; }
            public double SignalRadius { get; set; }
            public double SignalArea { get; set; }
            public string RawSecurityPayload { get; set; }
            public string SecurityStandards { get; set; }
            public string SecurityProtocols { get; set; }
            public bool IsSecure { get; set; }
            public bool Status { get; set; }
            public Guid? RunIdentifier { get; set; }
        }

        public class AccessPointAdnnotationDetails
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }

        public class AccessPointPacketDetails
        {
            public Guid Id { get; set; }
            public string DestinationAddress { get; set; }
            public string FrameType { get; set; }
            public string Data { get; set; }
        }
    }
}
