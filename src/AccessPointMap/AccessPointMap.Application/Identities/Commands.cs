using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Domain.Identities;
using System;

namespace AccessPointMap.Application.Identities
{
    public static class Commands
    {
        public static class V1
        {
            public class Delete : IApplicationCommand<Identity>
            {
                public Guid Id { get; set; }
            }

            public class Activation : IApplicationCommand<Identity>
            {
                public Guid Id { get; set; }

                public bool? Activated { get; set; }
            }

            public class RoleChange : IApplicationCommand<Identity>
            {
                public Guid Id { get; set; }

                public UserRole? Role { get; set; }
            }
        }
    }
}
