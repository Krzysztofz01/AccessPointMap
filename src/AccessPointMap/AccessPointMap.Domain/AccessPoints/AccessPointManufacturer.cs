using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointManufacturer : ValueObject<AccessPointManufacturer>
    {
        public string Value { get; private set; }

        private AccessPointManufacturer() { }
        private AccessPointManufacturer(string value)
        {
            if (value.IsEmpty())
                Value = string.Empty;

            Value = value;
        }

        public static implicit operator string(AccessPointManufacturer manufacturer) => manufacturer.Value;

        public static AccessPointManufacturer FromString(string manufacturer) => new AccessPointManufacturer(manufacturer);
        public static AccessPointManufacturer Unknown => new AccessPointManufacturer(string.Empty);
    }
}
