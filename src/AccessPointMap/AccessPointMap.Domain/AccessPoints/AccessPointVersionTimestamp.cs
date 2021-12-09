using AccessPointMap.Domain.Shared;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointVersionTimestamp : Timestamp
    {
        protected AccessPointVersionTimestamp() { }
        protected AccessPointVersionTimestamp(DateTime value) : base(value) { }

        public static implicit operator DateTime(AccessPointVersionTimestamp modificationTimestamp) => modificationTimestamp.Value;

        public static AccessPointVersionTimestamp FromDateTime(DateTime date) => new AccessPointVersionTimestamp(date);
        public static AccessPointVersionTimestamp Now => new AccessPointVersionTimestamp(DateTime.Now);
    }
}
