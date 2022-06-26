namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal enum Ieee80211SubTypes: ushort
    {
        BlockAck = 0x9400,
        Acknowledgement = 0xd400,
        Action = 0xd000,
        Beacon = 0x8000,
        ClearToSend = 0xc400,
        Data = 0x0842,
        NullFunction = 0x4811,
        Probe = 0x5000,
        Qos = 0x8851,
        RequestToSend = 0xb400
    }
}
