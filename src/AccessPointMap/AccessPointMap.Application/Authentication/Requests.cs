namespace AccessPointMap.Application.Authentication
{
    public static class Requests
    {
        public static class V1
        {
            public class Login : IAuthenticationRequest
            {
                public string Email { get; set; }

                public string Password { get; set; }
            }

            public class Register : IAuthenticationRequest
            {
                public string Email { get; set; }

                public string Name { get; set; }

                public string Password { get; set; }

                public string PasswordRepeat { get; set; }
            }

            public class Logout : IAuthenticationRequest
            {
                public string RefreshToken { get; set; }
            }

            public class PasswordReset : IAuthenticationRequest
            {
                public string Password { get; set; }

                public string PasswordRepeat { get; set; }
            }

            public class Refresh : IAuthenticationRequest
            {
                public string RefreshToken { get; set; }
            }
        }
    }
}
