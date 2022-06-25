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

        public const int Ieee80211ActionDestinationOffset = 4;
        public const int Ieee80211ActionSourceOffset = 10;

        public const int Ieee80211BeaconDestinationOffset = 4;
        public const int Ieee80211BeaconSourceOffset = 10;

        public const int Ieee80211DataDestinationOffset = 4;
        public const int Ieee80211DataSourceOffset = 16;

        public const int Ieee80211NullFuncDestinationOffset = 16;
        public const int Ieee80211NullFuncSourceOffset = 10;

        public const int Ieee80211ProbeDestinationOffset = 4;
        public const int Ieee80211ProbeSourceOffset = 10;

        public const int Ieee80211QosDestinationOffset = 16;
        public const int Ieee80211QosSourceOffset = 10;

        public const int Ieee80211RequestToSendDestinationOffset = 4;
        public const int Ieee80211RequestToSendSourceOffset = 10;
    }
}
