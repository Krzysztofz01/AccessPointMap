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
                //Skip pcap file header
                _ = _binaryReader.ReadBytes(Constants.PcapFileHeaderLength);

                var packets = new List<Packet>();

                while (_binaryReader.BaseStream.Position < _binaryReader.BaseStream.Length)
                    packets.Add(ParsePacket());

                return packets;
            }
            catch (Exception ex)
            {
                //TODO Error handling
                var exD = ex;
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

            var sourceAddressOffset = GetSourceOffset(subType);
            var sourceAddress = ieeeFrameBuffer.ToBase64HardwareAddress(sourceAddressOffset);

            var destinationAddressOffset = GetDestinationOffset(subType);
            var destinationAddress = ieeeFrameBuffer.ToBase64HardwareAddress(destinationAddressOffset);

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

        private int GetSourceOffset(Ieee80211SubTypes ieee80211SubTypes)
        {
            throw new NotImplementedException();
        }

        private int GetDestinationOffset(Ieee80211SubTypes ieee80211SubTypes)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_binaryReader is not null)
                _binaryReader.Close();
        }
    }
}
