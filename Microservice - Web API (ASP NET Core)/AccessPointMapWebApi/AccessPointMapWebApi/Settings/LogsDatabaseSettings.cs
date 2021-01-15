namespace AccessPointMapWebApi.Settings
{
    public interface ILogsDatabaseSettings
    {
        string LogCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class LogsDatabaseSettings : ILogsDatabaseSettings
    {
        public string LogCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
