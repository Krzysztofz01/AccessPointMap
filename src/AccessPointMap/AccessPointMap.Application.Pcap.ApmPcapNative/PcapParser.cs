using AccessPointMap.Application.Pcap.ApmPcapNative.Extensions;
using AccessPointMap.Application.Pcap.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal class PcapParser : IDisposable
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
                Console.WriteLine(ex);
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

            Ieee80211FrameTypes frameType = GetFrameType(ieeeFrameBuffer);

            var sourceAddress = GetSourceAddress(ieeeFrameBuffer, frameType);

            var destinationAddress = GetDestinationAddress(ieeeFrameBuffer, frameType);

            long frameEndPosition = _binaryReader.BaseStream.Position;

            _binaryReader.BaseStream.Position = frameStartPosition;

            int fullFrameLength = Constants.FrameHeaderLength + ieee80211FrameLength;

            byte[] fullFrameBuffer = _binaryReader.ReadBytes(fullFrameLength);
            var fullFrameBufferBase64 = Convert.ToBase64String(fullFrameBuffer);

            _binaryReader.BaseStream.Position = frameEndPosition;

            return new Packet()
            {
                Data = fullFrameBufferBase64,
                FrameType = Enum.GetName(frameType),
                DestinationAddress = destinationAddress,
                SourceAddress = sourceAddress
            };
        }

        private static string GetSourceAddress(byte[] ieeeFrameBuffer, Ieee80211FrameTypes frameTypes)
        {
            int? offset = frameTypes switch
            {
                Ieee80211FrameTypes.Management => Constants.Ieee80211ManagementFrameSourceOffset,
                Ieee80211FrameTypes.Control => Constants.Ieee80211ControlFrameSourceOffset,
                Ieee80211FrameTypes.Data => Constants.Ieee80211DataFrameSourceOffset,
                Ieee80211FrameTypes.Extension => null,
                _ => throw new ArgumentOutOfRangeException(nameof(frameTypes)),
            };

            if (offset is null) return string.Empty;

            var addressBuffer = ieeeFrameBuffer.ToHardwareAddressBuffer(offset.Value);

            //TODO: Better handling of len_diff6 edge case
            return addressBuffer.Length == 6
                ? GetHardwareAddressString(addressBuffer)
                : string.Empty;
        }

        private static string GetDestinationAddress(byte[] ieeeFrameBuffer, Ieee80211FrameTypes frameTypes)
        {
            int? offset = frameTypes switch
            {
                Ieee80211FrameTypes.Management => Constants.Ieee80211ManagementFrameDestinationOffset,
                Ieee80211FrameTypes.Control => Constants.Ieee80211ControlFrameDestinationOffset,
                Ieee80211FrameTypes.Data => Constants.Ieee80211DataFrameDestinationOffset,
                Ieee80211FrameTypes.Extension => null,
                _ => throw new ArgumentOutOfRangeException(nameof(frameTypes)),
            };

            if (offset is null) return string.Empty;

            var addressBuffer = ieeeFrameBuffer.ToHardwareAddressBuffer(offset.Value);

            //TODO: Better handling of len_diff6 edge case
            return addressBuffer.Length == 6
                ? GetHardwareAddressString(addressBuffer)
                : string.Empty;
        }

        private static Ieee80211FrameTypes GetFrameType(byte[] ieeeFrameBuffer)
        {
            var frameControlField = ieeeFrameBuffer.ToUInt16(0, true);

            var msb = frameControlField.GetBit(9);
            var lsb = frameControlField.GetBit(8);

            if (!msb && !lsb) return Ieee80211FrameTypes.Management;
            if (!msb && lsb) return Ieee80211FrameTypes.Control;
            if (msb && !lsb) return Ieee80211FrameTypes.Data;
            return Ieee80211FrameTypes.Extension;
        }

        private static string GetHardwareAddressString(byte[] hardwareAddresBuffer)
        {
            if (hardwareAddresBuffer.Length != 6)
                throw new ArgumentException("Invalid hardware address buffer format.", nameof(hardwareAddresBuffer));

            var hardwareAddressBuilder = new StringBuilder(string.Empty);
            foreach (var addressByte in hardwareAddresBuffer)
            {
                hardwareAddressBuilder.Append(addressByte.ToString("X2").ToUpper());
                hardwareAddressBuilder.Append(':');
            }
            
            return hardwareAddressBuilder.ToString().SkipLast();
        }

        public void Dispose()
        {
            if (_binaryReader is not null)
                _binaryReader.Close();

            GC.SuppressFinalize(this);
        }
    }
}
