namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal enum Ieee80211SubTypes: ushort
    {
        BlockAck = 0x0019,
        Acknowledgement = 0x001d,
        Action = 0x000d,
        Beacon = 0x0008,
        ClearToSend = 0x001c,
        Data = 0x0020,
        NullFunction = 0x0024,
        Probe = 0x0005,
        Qos = 0x0028,
        RequestToSend = 0x001b
    }
}
