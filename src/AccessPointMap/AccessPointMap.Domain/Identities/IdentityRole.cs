using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.Identities
{
    public class IdentityRole : ValueObject<IdentityRole>
    {
        public UserRole Value { get; private set; }

        private IdentityRole() { }
        private IdentityRole(UserRole value)
        {
            Value = value;
        }

        public static implicit operator string(IdentityRole identityRole) => Enum.GetName(typeof(UserRole), identityRole.Value);
        public static implicit operator UserRole(IdentityRole identityRole) => identityRole.Value;

        public static IdentityRole Default => new IdentityRole(UserRole.Default);
        public static IdentityRole FromUserRole(UserRole userRole) => new IdentityRole(userRole);
    }

    public enum UserRole
    {
        Default,
        Admin,
        Support,
        Bot
    }
}
