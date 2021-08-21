using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Web.ViewModels
{
    public class AccessPointIntegrationPostView
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
