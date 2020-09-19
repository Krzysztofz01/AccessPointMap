using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ms_accesspointmap_api.Models
{
    public partial class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public int TokenExpiration { get; set; }
        public bool WritePermission { get; set; }
        public bool ReadPermission { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public bool? Active { get; set; }
    }
}
