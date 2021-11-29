using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointDisplayStatus : ValueObject<AccessPointDisplayStatus>
    {
        public bool Value { get; private set; }

        private AccessPointDisplayStatus() { }
        private AccessPointDisplayStatus(bool value)
        {
            Value = value;
        }

        public static implicit operator bool(AccessPointDisplayStatus displayStatus) => displayStatus.Value;

        public static AccessPointDisplayStatus Visible => new AccessPointDisplayStatus(true);
        public static AccessPointDisplayStatus Hidden => new AccessPointDisplayStatus(false);
        public static AccessPointDisplayStatus FromBool(bool displayStatus) => new AccessPointDisplayStatus(displayStatus);
    }
}
