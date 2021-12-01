using AccessPointMap.Domain.Shared;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointModificationTimestamp : Timestamp
    {
        protected AccessPointModificationTimestamp() { }
        protected AccessPointModificationTimestamp(DateTime value) : base(value) { }

        public static implicit operator DateTime(AccessPointModificationTimestamp modificationTimestamp) => modificationTimestamp.Value;

        public static AccessPointModificationTimestamp FromDateTime(DateTime date) => new AccessPointModificationTimestamp(date);
        public static AccessPointModificationTimestamp Now => new AccessPointModificationTimestamp(DateTime.Now);
    }
}
