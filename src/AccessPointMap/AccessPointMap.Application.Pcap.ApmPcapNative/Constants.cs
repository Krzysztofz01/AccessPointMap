namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal class Constants
    {
        public const int PcapFileHeaderLength = 24;

        public const int FrameHeaderStartToLengthOffset = 8;

        public const int FrameHeaderLengthToEndOffset = 6;

        public const int FrameHeaderLength = FrameHeaderStartToLengthOffset + FrameHeaderLengthToEndOffset + sizeof(ushort);

        public const int Ieee80211BlockAckDestinationOffset = 4;
        public const int Ieee80211BlockAckSourceOffset = 10;
    }
}
