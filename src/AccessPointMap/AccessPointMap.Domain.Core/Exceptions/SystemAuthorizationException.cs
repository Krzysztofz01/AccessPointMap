using System;
using System.Runtime.Serialization;

namespace AccessPointMap.Domain.Core.Exceptions
{
    [Obsolete("This exception is not and extension to the domain exception and should not be used.")]
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

        [Obsolete("This factory should not be used beacause of stack trace loss.")]
        public static void Unauthenticated() => throw new SystemAuthorizationException("No permission to access this resource.");
    }
}
