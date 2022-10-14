namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal static class Constants
    {
        public const int PcapFileHeaderLength = 24;

        public const int FrameHeaderStartToLengthOffset = 8;

        public const int FrameHeaderLengthToEndOffset = 6;

        public const int FrameHeaderLength = FrameHeaderStartToLengthOffset + FrameHeaderLengthToEndOffset + sizeof(ushort);

        public const int Ieee80211ManagementFrameDestinationOffset = 4;
        public const int Ieee80211ManagementFrameSourceOffset = 10;

        public const int Ieee80211ControlFrameDestinationOffset = 4;
        public const int Ieee80211ControlFrameSourceOffset = 10;

        public const int Ieee80211DataFrameDestinationOffset = 4;
        public const int Ieee80211DataFrameSourceOffset = 16;

        //TODO: Implement extensions related offsets (if needed)
    }
}
