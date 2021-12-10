using Microsoft.AspNetCore.Http;

namespace AccessPointMap.Application.Integration.Wigle
{
    public static class Requests
    {
        public class Create
        {
            public IFormFile CsvDatabaseFile { get; set; }
        }
    }
}
