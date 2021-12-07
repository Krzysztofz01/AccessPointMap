namespace AccessPointMap.Application.Authentication
{
    public static class Responses
    {
        public static class V1
        {
            public class Login
            {
                public string JsonWebToken { get; set; }
            }

            public class Refresh
            {
                public string JsonWebToken { get; set; }
            }
        }
    }
}
