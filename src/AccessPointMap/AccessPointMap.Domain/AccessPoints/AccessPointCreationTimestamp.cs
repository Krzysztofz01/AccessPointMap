using AccessPointMap.Domain.Shared;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointCreationTimestamp : Timestamp
    {
        protected AccessPointCreationTimestamp() { }
        protected AccessPointCreationTimestamp(DateTime value) : base(value) { }

        public static implicit operator DateTime(AccessPointCreationTimestamp creationTimestamp) => creationTimestamp.Value;

        public static AccessPointCreationTimestamp FromDateTime(DateTime date) => new AccessPointCreationTimestamp(date);
    }
}
