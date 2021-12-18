using AccessPointMap.Domain.Core.Events;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public static class Events
    {
        public static class V1
        {
            public class AccessPointCreated : IEventBase
            {
                public string Bssid { get; set; }
                public string Ssid { get; set; }
                public double Frequency { get; set; }
                public int LowSignalLevel { get; set; }
                public double LowSignalLatitude { get; set; }
                public double LowSignalLongitude { get; set; }
                public int HighSignalLevel { get; set; }
                public double HighSignalLatitude { get; set; }
                public double HighSignalLongitude { get; set; }
                public string RawSecurityPayload { get; set; }
                public Guid UserId { get; set; }
            }

            public class AccessPointDeleted : IEvent
            {
                public Guid Id { get; set; }
            }

            public class AccessPointUpdated : IEvent
            {
                public Guid Id { get; set; }
                public string Note { get; set; }
            }

            public class AccessPointDisplayStatusChanged : IEvent
            {
                public Guid Id { get; set; }
                public bool Status { get; set; }
            }

            public class AccessPointManufacturerChanged : IEvent
            {
                public Guid Id { get; set; }
                public string Manufacturer { get; set; }
            }

            public class AccessPointMergedWithStamp : IEvent
            {
                public Guid Id { get; set; }
                public Guid StampId { get; set; }
                public bool MergeLowSignalLevel { get; set; }
                public bool MergeHighSignalLevel { get; set; }
                public bool MergeSsid { get; set; }
                public bool MergeSecurityData { get; set; }
            }

            public class AccessPointStampCreated : IEvent
            {
                public Guid Id { get; set; }
                public string Ssid { get; set; }
                public double Frequency { get; set; }
                public int LowSignalLevel { get; set; }
                public double LowSignalLatitude { get; set; }
                public double LowSignalLongitude { get; set; }
                public int HighSignalLevel { get; set; }
                public double HighSignalLatitude { get; set; }
                public double HighSignalLongitude { get; set; }
                public string RawSecurityPayload { get; set; }
                public Guid UserId { get; set; }
            }

            public class AccessPointStampDeleted : IEvent
            {
                public Guid Id { get; set; }
                public Guid StampId { get; set; }
            }
        }
    }
}
