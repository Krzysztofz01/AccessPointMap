using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.Identities
{
    public class IdentityPasswordHash : ValueObject<IdentityPasswordHash>
    {
        public string Value { get; private set; }

        private IdentityPasswordHash() { }
        private IdentityPasswordHash(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationException("The identity password hash length is invalid");

            Value = value;
        }

        public static implicit operator string(IdentityPasswordHash passwordHash) => passwordHash.Value;

        public static IdentityPasswordHash FromString(string passwordHash) => new IdentityPasswordHash(passwordHash);
    }
}
