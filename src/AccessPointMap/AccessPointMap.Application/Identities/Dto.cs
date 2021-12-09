using System;

namespace AccessPointMap.Application.Identities
{
    public static class Dto
    {
        public class IdentitySimple
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Role { get; set; }
        }

        public class IdentityDetails
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string LastLoginIp { get; set; }
            public DateTime LastLoginDate { get; set; }
            public string Role { get; set; }
            public bool Activation { get; set; }
        }
    }
}
