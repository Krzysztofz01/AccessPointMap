using System;
using System.Runtime.Serialization;

namespace AccessPointMap.Application.Integration.Core.Exceptions
{
    public class AccessPointIntegrationException : IntegrationException
    {
        public AccessPointIntegrationException()
        {
        }

        public AccessPointIntegrationException(string message) : base(message)
        {
        }

        public AccessPointIntegrationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AccessPointIntegrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
