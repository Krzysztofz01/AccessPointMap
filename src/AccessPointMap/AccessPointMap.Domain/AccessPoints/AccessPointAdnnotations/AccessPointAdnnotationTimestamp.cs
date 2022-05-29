using AccessPointMap.Domain.Shared;
using System;

namespace AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations
{
    public class AccessPointAdnnotationTimestamp : Timestamp 
    {
        protected AccessPointAdnnotationTimestamp() { }
        protected AccessPointAdnnotationTimestamp(DateTime value) : base(value) { }

        public static implicit operator DateTime(AccessPointAdnnotationTimestamp adnnotationTimestamp) => adnnotationTimestamp.Value;

        public static AccessPointAdnnotationTimestamp Now => new AccessPointAdnnotationTimestamp(DateTime.Now);
    }
}
