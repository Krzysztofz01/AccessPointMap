using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ms_accesspointmap_api.Models
{
    public partial class Accesspoints
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the BSSID")]
        public string Bssid { get; set; }
        [Required(ErrorMessage = "Please enter the SSID")]
        public string Ssid { get; set; }
        [Required(ErrorMessage = "Please enter the Frequency")]
        public int Frequency { get; set; }
        [Required(ErrorMessage = "Please enter the HighSignalLevel")]
        public int HighSignalLevel { get; set; }
        [Required(ErrorMessage = "Please enter the HighLongitude")]
        public double HighLongitude { get; set; }
        [Required(ErrorMessage = "Please enter the HighLatitude")]
        public double HighLatitude { get; set; }
        [Required(ErrorMessage = "Please enter the LowSignalLevel")]
        public int LowSignalLevel { get; set; }
        [Required(ErrorMessage = "Please enter the LowLongitude")]
        public double LowLongitude { get; set; }
        [Required(ErrorMessage = "Please enter the LowLatitude")]
        public double LowLatitude { get; set; }
        public double SignalRadius { get; set; }
        public double SignalArea { get; set; }
        [Required(ErrorMessage = "Please enter the SecurityData")]
        public string SecurityData { get; set; }
        public string Brand { get; set; }
        public string DeviceType { get; set; }
        public bool? Display { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
