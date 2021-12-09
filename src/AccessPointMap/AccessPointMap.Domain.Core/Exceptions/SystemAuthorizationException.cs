using System;
using System.Runtime.Serialization;

namespace AccessPointMap.Domain.Core.Exceptions
{
    public class SystemAuthorizationException : Exception
    {
        public SystemAuthorizationException()
        {
        }

        public SystemAuthorizationException(string message) : base(message)
        {
        }

        public SystemAuthorizationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SystemAuthorizationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static void Unauthenticated() => throw new SystemAuthorizationException("No permission to access this resource.");
    }
}
