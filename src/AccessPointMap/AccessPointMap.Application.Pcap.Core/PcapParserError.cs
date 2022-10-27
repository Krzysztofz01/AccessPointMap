using AccessPointMap.Application.Abstraction;

namespace AccessPointMap.Application.Pcap.Core
{
    public class PcapParserError : Error
    {
        protected PcapParserError() { }
        protected PcapParserError(string message) : base(message) { }

        public static PcapParserError UploadedPcapFileIsNull => new("The provided PCAP/CAP file can not be accessed.");
        public static PcapParserError UploadedFileHasInvalidFormat => new("The provided file format is not matching the requirements.");
        public static PcapParserError FatalError => new("The CAP/PCAP format parser encountered an unexpected error.");
    }
}
