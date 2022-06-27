namespace AccessPointMap.Application.Pcap.Core
{
    public class Packet
    {
        public string SourceAddress { get; init; }
        public string DestinationAddress { get; init; }
        public string FrameType { get; set; }
        public string Data { get; init; }
    }
}
