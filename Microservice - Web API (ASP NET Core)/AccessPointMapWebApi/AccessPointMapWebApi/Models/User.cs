using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AccessPointMapWebApi.Models
{
    public partial class User
    {
        [Key]
        public int Id { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int TokenExpiration { get; set; }

        public bool? WritePermission { get; set; }

        public bool? ReadPermission { get; set; }

        public bool? AdminPermission { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public string LastLoginIp { get; set; }

        public bool? Active { get; set; }
    }

    public class UserFormDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Must be between 5 and 128 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserActivationDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool Active { get; set; }
    }

    public class UserResponseDto
    {
        public string Email { get; set; }
        public int TokenExpiration { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
