using AccessPointMap.Application.Pcap.ApmPcapNative.Extensions;
using AccessPointMap.Application.Pcap.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    // Public for development purposes, change to internal
    public class PcapParser : IDisposable
    {
        private readonly BinaryReader _binaryReader;

        private PcapParser() { }

        public PcapParser(Stream pcapFileStream)
        {
            if (pcapFileStream is null)
                throw new ArgumentNullException(nameof(pcapFileStream));

            _binaryReader = new BinaryReader(pcapFileStream);
        }

        public IEnumerable<Packet> ParsePackets()
        {
            try
            {
                _binaryReader.SkipBytes(Constants.PcapFileHeaderLength);

                var packets = new List<Packet>();

                while (_binaryReader.BaseStream.Position < _binaryReader.BaseStream.Length)
                    packets.Add(ParsePacket());

                return packets;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return null;
            }
        }

        private Packet ParsePacket()
        {
            long frameStartPosition = _binaryReader.BaseStream.Position;

            _binaryReader.SkipBytes(Constants.FrameHeaderStartToLengthOffset);

            ushort ieee80211FrameLength = _binaryReader.ReadUInt16();

            _binaryReader.SkipBytes(Constants.FrameHeaderLengthToEndOffset);

            byte[] ieeeFrameBuffer = _binaryReader.ReadBytes(ieee80211FrameLength);

            Ieee80211SubTypes subType = (Ieee80211SubTypes)ieeeFrameBuffer.ToUInt16();

            var sourceAddress = GetSourceAddress(ieeeFrameBuffer, subType);

            var destinationAddress = GetDestinationAddress(ieeeFrameBuffer, subType);

            long frameEndPosition = _binaryReader.BaseStream.Position;

            _binaryReader.BaseStream.Position = frameStartPosition;

            int fullFrameLength = Constants.FrameHeaderLength + ieee80211FrameLength;

            byte[] fullFrameBuffer = _binaryReader.ReadBytes(fullFrameLength);
            var fullFrameBufferBase64 = Convert.ToBase64String(fullFrameBuffer);

            _binaryReader.BaseStream.Position = frameEndPosition;

            return new Packet()
            {
                Data = fullFrameBufferBase64,
                DestinationAddress = destinationAddress,
                SourceAddress = sourceAddress
            };
        }

        private static string GetSourceAddress(byte[] ieeeFrameBuffer, Ieee80211SubTypes ieee80211SubTypes)
        {
            int? offset = ieee80211SubTypes switch
            {
                Ieee80211SubTypes.BlockAck => Constants.Ieee80211BlockAckSourceOffset,
                Ieee80211SubTypes.Acknowledgement => null,
                Ieee80211SubTypes.Action => Constants.Ieee80211ActionSourceOffset,
                Ieee80211SubTypes.Beacon => Constants.Ieee80211BeaconSourceOffset,
                Ieee80211SubTypes.ClearToSend => null,
                Ieee80211SubTypes.Data => Constants.Ieee80211DataSourceOffset,
                Ieee80211SubTypes.NullFunction => Constants.Ieee80211NullFuncSourceOffset,
                Ieee80211SubTypes.Probe => Constants.Ieee80211ProbeSourceOffset,
                Ieee80211SubTypes.Qos => Constants.Ieee80211QosSourceOffset,
                Ieee80211SubTypes.RequestToSend => Constants.Ieee80211RequestToSendSourceOffset,
                _ => throw new ArgumentOutOfRangeException(nameof(ieee80211SubTypes)),
            };

            if (offset is null) return string.Empty;

            var addressBuffer = ieeeFrameBuffer.ToHardwareAddressBuffer(offset.Value);
            return Convert.ToBase64String(addressBuffer);
        }

        private static string GetDestinationAddress(byte[] ieeeFrameBuffer, Ieee80211SubTypes ieee80211SubTypes)
        {
            int? offset = ieee80211SubTypes switch
            {
                Ieee80211SubTypes.BlockAck => Constants.Ieee80211BlockAckDestinationOffset,
                Ieee80211SubTypes.Acknowledgement => null,
                Ieee80211SubTypes.Action => Constants.Ieee80211ActionDestinationOffset,
                Ieee80211SubTypes.Beacon => Constants.Ieee80211BeaconDestinationOffset,
                Ieee80211SubTypes.ClearToSend => null,
                Ieee80211SubTypes.Data => Constants.Ieee80211DataDestinationOffset,
                Ieee80211SubTypes.NullFunction => Constants.Ieee80211NullFuncDestinationOffset,
                Ieee80211SubTypes.Probe => Constants.Ieee80211ProbeDestinationOffset,
                Ieee80211SubTypes.Qos => Constants.Ieee80211QosDestinationOffset,
                Ieee80211SubTypes.RequestToSend => Constants.Ieee80211RequestToSendDestinationOffset,
                _ => throw new ArgumentOutOfRangeException(nameof(ieee80211SubTypes)),
            };

            if (offset is null) return string.Empty;

            var addressBuffer = ieeeFrameBuffer.ToHardwareAddressBuffer(offset.Value);
            return Convert.ToBase64String(addressBuffer);
        }

        public void Dispose()
        {
            if (_binaryReader is not null)
                _binaryReader.Close();

            GC.SuppressFinalize(this);
        }
    }
}
