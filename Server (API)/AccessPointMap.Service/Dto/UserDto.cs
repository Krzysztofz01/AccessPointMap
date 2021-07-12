using System;
using System.Collections.Generic;

namespace AccessPointMap.Service.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime? DeleteDate { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool AdminPermission { get; set; }
        public bool ModPermission { get; set; }
        public bool IsActivated { get; set; }
        public IEnumerable<AccessPointDto> AddedAccessPoints { get; set; }
        public IEnumerable<AccessPointDto> ModifiedAccessPoints { get; set; }
    }
}
