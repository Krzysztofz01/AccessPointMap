namespace AccessPointMap.Application.Settings
{
    public class DatabaseSettings
    {
        public string Driver { get; set; }
        public string ConnectionString { get; set; }
        public bool ApplyMigrations { get; set; }

    }
}
