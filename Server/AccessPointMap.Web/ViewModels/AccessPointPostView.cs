using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Web.ViewModels
{
    public class AccessPointPostView
    {
        [Required]
        public string Bssid { get; set; }

        public string Ssid { get; set; }

        [Required]
        public double Frequency { get; set; }

        [Required]
        public int MaxSignalLevel { get; set; }

        [Required]
        public double MaxSignalLongitude { get; set; }

        [Required]
        public double MaxSignalLatitude { get; set; }

        [Required]
        public int MinSignalLevel { get; set; }

        [Required]
        public double MinSignalLongitude { get; set; }

        [Required]
        public double MinSignalLatitude { get; set; }

        [Required]
        public string FullSecurityData { get; set; }
    }
}
