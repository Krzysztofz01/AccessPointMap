using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.Shared
{
    public class Identifier : ValueObject<Identifier>
    {
        public Guid Value { get; protected set; }

        protected Identifier() { }
        protected Identifier(Guid value)
        {
            if (value == default)
                throw new ValueObjectValidationException("The identifier value can not be default.");

            Value = value;
        }
    }
}
