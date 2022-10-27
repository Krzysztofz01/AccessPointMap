using AccessPointMap.Application.Integration.Core;

namespace AccessPointMap.Application.Integration.Wireshark
{
    public class WiresharkIntegrationError : IntegrationError
    {
        protected WiresharkIntegrationError(string message) : base(message) { }

        public static WiresharkIntegrationError UploadedPcapFileIsNull => new("The provided CAP/PCAP file can not be accessed.");
        public static WiresharkIntegrationError UploadedFileHasInvalidFormat => new("The provided file format is not matching the requirements.");
    }
}
