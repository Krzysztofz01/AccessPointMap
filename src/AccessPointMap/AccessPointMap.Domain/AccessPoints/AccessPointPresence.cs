using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointPresence : ValueObject<AccessPointPresence>
    {
        public bool Value { get; private set; }

        private AccessPointPresence(bool value)
        {
            Value = value;
        }

        public static implicit operator bool(AccessPointPresence presence) => presence.Value;

        public static AccessPointPresence FromBool(bool presence) => new AccessPointPresence(presence);
        public static AccessPointPresence Present => new AccessPointPresence(true);

    }
}
