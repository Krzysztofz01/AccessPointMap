namespace AccessPointMap.Application.Authentication
{
    public static class Requests
    {
        public static class V1
        {
            public class Login
            {
                public string Email { get; set; }

                public string Password { get; set; }
            }

            public class Register
            {
                public string Email { get; set; }

                public string Name { get; set; }

                public string Password { get; set; }

                public string PasswordRepeat { get; set; }
            }

            public class Logout
            {
                public string RefreshToken { get; set; }
            }

            public class PasswordReset
            {
                public string Password { get; set; }

                public string PasswordRepeat { get; set; }
            }

            public class Refresh
            {
                public string RefreshToken { get; set; }
            }
        }
    }
}
