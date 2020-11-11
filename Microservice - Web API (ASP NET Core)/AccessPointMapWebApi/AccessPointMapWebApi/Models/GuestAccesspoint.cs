using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AccessPointMapWebApi.Models
{
    public partial class GuestAccesspoint
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

        [Required]
        public string SecurityDataRaw { get; set; }

        public string DeviceType { get; set; }

        [Required]
        public string PostedBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
