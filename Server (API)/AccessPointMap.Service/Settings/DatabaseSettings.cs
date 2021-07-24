namespace AccessPointMap.Service.Settings
{
    public class DatabaseSettings
    {
        public bool UseDefaultProvider { get; set; }
        public string SqlServerConnectionString { get; set; }
        public string MySqlConnectionString { get; set; }
    }
}
