using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.Shared
{
    public class Timestamp : ValueObject<Timestamp>
    {
        public DateTime Value { get; protected set; }

        protected Timestamp() { }
        protected Timestamp(DateTime value)
        {
            if (value == default)
                throw new ValueObjectValidationException("The timestamp can not have the default value.");

            Value = value;
        }
    }
}
