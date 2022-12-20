using AccessPointMap.Application.Core;

namespace AccessPointMap.Application.Oui.Core
{
    public class OuiServiceError : Error
    {
        protected OuiServiceError() { }
        protected OuiServiceError(string message) : base(message) { }

        public static OuiServiceError OuiNotFound => new("No vendor found based on the provided MAC address.");
        public static OuiServiceError FatalError => new("The OUI data provider service encountered an unexpected error.");
    }
}
