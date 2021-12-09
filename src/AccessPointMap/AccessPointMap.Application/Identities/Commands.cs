using AccessPointMap.Application.Abstraction;
using AccessPointMap.Domain.Identities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Identities
{
    public static class Commands
    {
        public static class V1
        {
            public class Delete : IApplicationCommand<Identity>
            {
                [Required]
                public Guid Id { get; set; }
            }

            public class Activation : IApplicationCommand<Identity>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public bool Activated { get; set; }
            }

            public class RoleChange : IApplicationCommand<Identity>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public UserRole Role { get; set; }
            }
        }
    }
}
