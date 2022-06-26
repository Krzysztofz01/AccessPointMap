using AccessPointMap.Application.Pcap.ApmPcapNative.Extensions;
using AccessPointMap.Application.Pcap.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
                //System.Diagnostics.Debug.WriteLine(ex);
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
            //Ieee80211SubTypes subType = (Ieee80211SubTypes)ieeeFrameBuffer.ToUInt16(0, true);

            var sourceAddress = GetSourceAddress(ieeeFrameBuffer, frameType);

            var destinationAddress = GetDestinationAddress(ieeeFrameBuffer, frameType);

            long frameEndPosition = _binaryReader.BaseStream.Position;

            _binaryReader.BaseStream.Position = frameStartPosition;

            int fullFrameLength = Constants.FrameHeaderLength + ieee80211FrameLength;

            byte[] fullFrameBuffer = _binaryReader.ReadBytes(fullFrameLength);
            var fullFrameBufferBase64 = Convert.ToBase64String(fullFrameBuffer);

            _binaryReader.BaseStream.Position = frameEndPosition;

            var packet = new Packet()
            {
                Data = fullFrameBufferBase64,
                DestinationAddress = destinationAddress,
                SourceAddress = sourceAddress
            };

            Console.WriteLine("{0} - {1} - {2} - {3}", sourceAddress, destinationAddress, Enum.GetName<Ieee80211FrameTypes>(frameType),fullFrameBufferBase64.Length);

            return packet;

            /*return new Packet()
            {
                Data = fullFrameBufferBase64,
                DestinationAddress = destinationAddress,
                SourceAddress = sourceAddress
            };*/
        }

        private static string GetSourceAddress(byte[] ieeeFrameBuffer, Ieee80211FrameTypes frameTypes)
        {
            //int? offset = ieee80211SubTypes switch
            //{
            //    Ieee80211SubTypes.BlockAck => Constants.Ieee80211BlockAckSourceOffset,
            //    Ieee80211SubTypes.Acknowledgement => null,
            //    Ieee80211SubTypes.Action => Constants.Ieee80211ActionSourceOffset,
            //    Ieee80211SubTypes.Beacon => Constants.Ieee80211BeaconSourceOffset,
            //    Ieee80211SubTypes.ClearToSend => null,
            //    Ieee80211SubTypes.Data => Constants.Ieee80211DataSourceOffset,
            //    Ieee80211SubTypes.NullFunction => Constants.Ieee80211NullFuncSourceOffset,
            //    Ieee80211SubTypes.Probe => Constants.Ieee80211ProbeSourceOffset,
            //    Ieee80211SubTypes.Qos => Constants.Ieee80211QosSourceOffset,
            //    Ieee80211SubTypes.RequestToSend => Constants.Ieee80211RequestToSendSourceOffset,
            //    _ => throw new ArgumentOutOfRangeException(nameof(ieee80211SubTypes)),
            //};
            
            int? offset = frameTypes switch
            {
                Ieee80211FrameTypes.Management => Constants.Ieee80211ManagementFrameSourceOffset,
                Ieee80211FrameTypes.Control => null,
                Ieee80211FrameTypes.Data => Constants.Ieee80211DataSourceOffset,
                Ieee80211FrameTypes.Extension => null,
                _ => throw new ArgumentOutOfRangeException(nameof(frameTypes)),
            };

            if (offset is null) return string.Empty;

            var addressBuffer = ieeeFrameBuffer.ToHardwareAddressBuffer(offset.Value);
            return GetHardwareAddressString(addressBuffer);
        }

        private static string GetDestinationAddress(byte[] ieeeFrameBuffer, Ieee80211FrameTypes frameTypes)
        {
            //int? offset = ieee80211SubTypes switch
            //{
            //    Ieee80211SubTypes.BlockAck => Constants.Ieee80211BlockAckDestinationOffset,
            //    Ieee80211SubTypes.Acknowledgement => null,
            //    Ieee80211SubTypes.Action => Constants.Ieee80211ActionDestinationOffset,
            //    Ieee80211SubTypes.Beacon => Constants.Ieee80211BeaconDestinationOffset,
            //    Ieee80211SubTypes.ClearToSend => null,
            //    Ieee80211SubTypes.Data => Constants.Ieee80211DataDestinationOffset,
            //    Ieee80211SubTypes.NullFunction => Constants.Ieee80211NullFuncDestinationOffset,
            //    Ieee80211SubTypes.Probe => Constants.Ieee80211ProbeDestinationOffset,
            //    Ieee80211SubTypes.Qos => Constants.Ieee80211QosDestinationOffset,
            //    Ieee80211SubTypes.RequestToSend => Constants.Ieee80211RequestToSendDestinationOffset,
            //    _ => throw new ArgumentOutOfRangeException(nameof(ieee80211SubTypes)),
            //};

            int? offset = frameTypes switch
            {
                Ieee80211FrameTypes.Management => Constants.Ieee80211ManagementFrameDestinationOffset,
                Ieee80211FrameTypes.Control => null,
                Ieee80211FrameTypes.Data => Constants.Ieee80211DataDestinationOffset,
                Ieee80211FrameTypes.Extension => null,
                _ => throw new ArgumentOutOfRangeException(nameof(frameTypes)),
            };

            if (offset is null) return string.Empty;

            var addressBuffer = ieeeFrameBuffer.ToHardwareAddressBuffer(offset.Value);
            return GetHardwareAddressString(addressBuffer);
        }

        private static Ieee80211FrameTypes GetFrameType(byte[] ieeeFrameBuffer)
        {
            var frameControlField = ieeeFrameBuffer.ToUInt16(0, true);

            var msb = frameControlField.GetBit(9);
            var lsb = frameControlField.GetBit(8);

            Console.WriteLine("{0}{1}", Convert.ToInt32(msb).ToString(), Convert.ToInt32(lsb).ToString());

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
