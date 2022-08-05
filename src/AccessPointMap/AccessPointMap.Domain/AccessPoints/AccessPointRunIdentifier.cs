using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointRunIdentifier : ValueObject<AccessPointRunIdentifier>
    {
        public Guid? Value { get; protected set; }

        protected AccessPointRunIdentifier() { }
        protected AccessPointRunIdentifier(Guid? value)
        {
            if (value.HasValue && value.Value == default)
                throw new ValueObjectValidationException("The identifier value can not be default.");

            Value = value;
        }

        public static implicit operator Guid?(AccessPointRunIdentifier runIdentifier) => runIdentifier.Value;

        public static AccessPointRunIdentifier FromGuid(Guid guid) => new AccessPointRunIdentifier(guid);
        public static AccessPointRunIdentifier None => new AccessPointRunIdentifier(null);
    }
}
