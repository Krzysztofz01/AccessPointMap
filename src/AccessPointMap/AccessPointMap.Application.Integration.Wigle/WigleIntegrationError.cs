using AccessPointMap.Application.Integration.Core;

namespace AccessPointMap.Application.Integration.Wigle
{
    internal class WigleIntegrationError : IntegrationError
    {
        protected WigleIntegrationError(string message) : base(message) { }

        public static WigleIntegrationError UploadedCsvFileIsNull => new("The provided CSV file can not be accessed.");
        public static WigleIntegrationError UploadedCsvGzFileIsNull => new("The provided CSV.GZ file can not be accessed.");
        public static WigleIntegrationError UploadedFileHasInvalidFormat => new("The provided file format is not matching the requirements.");
    }
}
