namespace AccessPointMap.Application.Pcap.Core
{
    public sealed class Packet
    {
        public string SourceAddress { get; init; }
        public string DestinationAddress { get; init; }
        public string FrameType { get; set; }
        public string Data { get; init; }
    }
}
