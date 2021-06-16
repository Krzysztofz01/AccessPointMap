using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Web.ViewModels
{
    public class AuthRequestReset
    {
        [Required]
        [MinLength(8, ErrorMessage = "Password length can't be less than 8.")]
        public string Password { get; set; }


        [Required]
        [MinLength(8, ErrorMessage = "Password length can't be less than 8.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords dont't match.")]
        public string PasswordRepeat { get; set; }
    }
}
