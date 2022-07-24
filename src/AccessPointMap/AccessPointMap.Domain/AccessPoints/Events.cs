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
                public Guid? RunIdentifier { get; set; }
                public Guid UserId { get; set; }
                public DateTime ScanDate { get; set; }
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

            public class AccessPointPresenceStatusChanged : IEvent
            {
                public Guid Id { get; set; }
                public bool Presence { get; set; }
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
                public Guid? RunIdentifier { get; set; }
                public Guid UserId { get; set; }
                public DateTime ScanDate { get; set; }
            }

            public class AccessPointStampDeleted : IEvent
            {
                public Guid Id { get; set; }
                public Guid StampId { get; set; }
            }

            public class AccessPointAdnnotationCreated : IEvent
            {
                public Guid Id { get; set; }
                public string Title { get; set; }
                public string Content { get; set; }
            }

            public class AccessPointAdnnotationDeleted : IEvent
            {
                public Guid Id { get; set; }
                public Guid AdnnotationId { get; set; }
            }

            public class AccessPointPacketCreated : IEvent
            {
                public Guid Id { get; set; }
                public string SourceAddress { get; set; }
                public string DestinationAddress { get; set; }
                public string FrameType { get; set; }
                public string Data { get; set; }
            }

            public class AccessPointPacketDeleted : IEvent
            {
                public Guid Id { get; set; }
                public Guid PacketId { get; set; }
            }
        }
    }
}
