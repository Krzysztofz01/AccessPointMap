using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Web.ViewModels
{
    public class AuthRequestLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
