using AccessPointMap.Application.Integration.Core;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    internal class AircrackngIntegrationError : IntegrationError
    {
        protected AircrackngIntegrationError(string message) : base(message) { }

        public static AircrackngIntegrationError UploadedCsvFileIsNull => new("The provided CSV file can not be accessed.");
        public static AircrackngIntegrationError UploadedPcapFileIsNull => new("The provided CAP/PCAP file can not be accessed.");
        public static AircrackngIntegrationError UploadedFileHasInvalidFormat => new("The provided file format is not matching the requirements.");
    }
}
