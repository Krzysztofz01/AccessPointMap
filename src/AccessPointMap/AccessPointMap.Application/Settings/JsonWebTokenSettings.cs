namespace AccessPointMap.Application.Settings
{
    public class JsonWebTokenSettings
    {
        public string TokenSecret { get; set; }
        public int BearerTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
}
