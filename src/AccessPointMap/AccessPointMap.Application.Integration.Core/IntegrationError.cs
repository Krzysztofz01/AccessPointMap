using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Domain.Core.Exceptions;

namespace AccessPointMap.Application.Integration.Core
{
    public class IntegrationError : Error
    {
        protected IntegrationError() { }

        protected IntegrationError(string message) : base(message) { }

        public static new IntegrationError Default => new("The integration service encountered an unexpected error.");
        public static IntegrationError FromDomainException(DomainException exception) => new(exception.Message);
        public static IntegrationError FromIntegrationException(IntegrationException exception) => new(exception.Message);
    }
}
