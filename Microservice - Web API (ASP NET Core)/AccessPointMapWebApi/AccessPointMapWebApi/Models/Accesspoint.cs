using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AccessPointMapWebApi.Models
{
    public partial class Accesspoint
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Bssid { get; set; }

        [Required]
        public string Ssid { get; set; }

        [Required]
        public int Frequency { get; set; }

        [Required]
        public int HighSignalLevel { get; set; }

        [Required]
        public double HighLongitude { get; set; }

        [Required]
        public double HighLatitude { get; set; }

        [Required]
        public int LowSignalLevel { get; set; }

        [Required]
        public double LowLongitude { get; set; }

        [Required]
        public double LowLatitude { get; set; }

        public double SignalRadius { get; set; }
        
        public double SignalArea { get; set; }

        public string SecurityData { get; set; }

        [Required]
        public string SecurityDataRaw { get; set; }

        public string Brand { get; set; }

        public string DeviceType { get; set; }

        public bool? Display { get; set; }

        [Required]
        public string PostedBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }

    public class AccessPointDisplayDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public bool Display { get; set; }
    }

    public class AccessPointBrandCountDto
    {
        public string Brand { get; set; }
        public int Count { get; set; }
    }
}
