using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Authentication
{
    public static class Requests
    {
        public static class V1
        {
            public class Login
            {
                [Required]
                public string Email { get; set; }


                [Required]
                public string Password { get; set; }
            }

            public class Register
            {
                [Required]
                public string Email { get; set; }

                [Required]
                public string Name { get; set; }

                [Required]
                public string Password { get; set; }

                [Required]
                public string PasswordRepeat { get; set; }
            }

            public class Logout
            {
            }

            public class PasswordReset
            {
                [Required]
                public string Password { get; set; }

                [Required]
                public string PasswordRepeat { get; set; }
            }

            public class Refresh
            {
            }
        }
    }
}
