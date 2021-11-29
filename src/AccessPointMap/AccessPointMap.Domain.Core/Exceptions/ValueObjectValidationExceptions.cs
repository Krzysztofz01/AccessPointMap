using System;
using System.Runtime.Serialization;

namespace AccessPointMap.Domain.Core.Exceptions
{
    public class ValueObjectValidationExceptions : Exception
    {
        public ValueObjectValidationExceptions()
        {
        }

        public ValueObjectValidationExceptions(string message) : base(message)
        {
        }

        public ValueObjectValidationExceptions(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ValueObjectValidationExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
