namespace AccessPointMap.Service.Settings
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public int TokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationsDays { get; set; }
    }
}
