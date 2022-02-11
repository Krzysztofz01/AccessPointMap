using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Integration.Wigle
{
    public static class Requests
    {
        public class Create
        {
            [Required]
            public IFormFile CsvDatabaseFile { get; set; }
        }
    }
}
