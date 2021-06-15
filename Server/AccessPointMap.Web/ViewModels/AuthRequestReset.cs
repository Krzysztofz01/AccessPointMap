using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Web.ViewModels
{
    public class AuthRequestReset
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordRepeat { get; set; }
    }
}
