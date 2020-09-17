using System;
using System.Collections.Generic;

namespace ms_accesspointmap_api.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public byte[] Password { get; set; }
        public int TokenExpiration { get; set; }
        public bool WritePermission { get; set; }
        public bool ReadPermission { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public bool? Active { get; set; }
    }
}
