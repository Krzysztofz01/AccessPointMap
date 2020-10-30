using System;
using System.Collections.Generic;

namespace ms_accesspointmap_api.Models
{
    public partial class Logs
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Endpoint { get; set; }
        public string Description { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
