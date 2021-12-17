using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.Integration.Aircrackng
{
    public static class Requests
    {
        public class Create
        {
            [Required]
            public IFormFile CsvDumpFile { get; set; }
        }
    }
}
