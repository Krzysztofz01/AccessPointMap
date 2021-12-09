using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.Identities
{
    public class IdentityActivation : ValueObject<IdentityActivation>
    {
        public bool Value { get; private set; }

        private IdentityActivation() { }
        private IdentityActivation(bool value)
        {
            Value = value;
        }

        public static implicit operator bool(IdentityActivation activation) => activation.Value;

        public static IdentityActivation FromBool(bool activation) => new IdentityActivation(activation);
        public static IdentityActivation Active => new IdentityActivation(true);
        public static IdentityActivation Inactive => new IdentityActivation(false);
    }
}
