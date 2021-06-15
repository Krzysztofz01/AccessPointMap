using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Web.ViewModels
{
    public class AuthRequestRegister
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string PasswordRepeat { get; set; }
    }
}
