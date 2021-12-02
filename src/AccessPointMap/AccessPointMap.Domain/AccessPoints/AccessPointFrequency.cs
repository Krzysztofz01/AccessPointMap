using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointFrequency : ValueObject<AccessPointFrequency>
    {
        private const double _default = 0.0;

        public double Value { get; private set; }

        private AccessPointFrequency() { }
        private AccessPointFrequency(double value)
        {
            if (value == default)
            {
                Value = _default;
                return;
            }

            Value = value;
        }

        public static implicit operator double(AccessPointFrequency frequency) => frequency.Value;

        public static AccessPointFrequency FromDouble(double frequency) => new AccessPointFrequency(frequency);
    }
}
