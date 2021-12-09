using AccessPointMap.Domain.Shared;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointContributorId : Identifier
    {
        protected AccessPointContributorId() { }
        protected AccessPointContributorId(Guid value) : base(value) { }

        public static implicit operator string(AccessPointContributorId contributorId) => contributorId.Value.ToString();
        public static implicit operator Guid(AccessPointContributorId contributorId) => contributorId.Value;

        public static AccessPointContributorId FromString(string guid) => new AccessPointContributorId(Guid.Parse(guid));
        public static AccessPointContributorId FromGuid(Guid guid) => new AccessPointContributorId(guid);
    }
}
