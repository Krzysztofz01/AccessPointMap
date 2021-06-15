using System;
using System.Collections.Generic;

namespace AccessPointMap.Domain
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool AdminPermission { get; set; }
        public bool ModPermission { get; set; }
        public bool IsActivated { get; set; }
        public virtual ICollection<AccessPoint> AddedAccessPoints { get; set; }
        public virtual ICollection<AccessPoint> ModifiedAccessPoints { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
