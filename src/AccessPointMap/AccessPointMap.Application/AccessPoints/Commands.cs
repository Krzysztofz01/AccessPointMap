using AccessPointMap.Application.Abstraction;
using AccessPointMap.Domain.AccessPoints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Commands
    {
        public static class V1
        {
            public class Create : IApplicationCommand<AccessPoint>
            {
                [Required]
                public IEnumerable<Helpers.AccessPointV1> AccessPoints { get; set; }

                [Required]
                public DateTime ScanDate { get; set; }
            }

            public class Delete : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }
            }

            public class Update : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }

                public string Note { get; set; }
            }

            public class ChangeDisplayStatus : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public bool Status { get; set; }
            }

            public class MergeWithStamp : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public Guid StampId { get; set; }

                [Required]
                public bool? MergeLowSignalLevel { get; set; }
                
                [Required]
                public bool? MergeHighSignalLevel { get; set; }
                
                [Required]
                public bool? MergeSsid { get; set; }
                
                [Required]
                public bool? MergeSecurityData { get; set; }
            }

            public class DeleteStamp : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public Guid StampId { get; set; }
            }

            public class CreateAdnnotation : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public string Title { get; set; }

                [Required]
                public string Content { get; set; }
            }
            
            public class DeleteAdnnotation : IApplicationCommand<AccessPoint>
            {
                [Required]
                public Guid Id { get; set; }

                [Required]
                public Guid AdnnotationId { get; set; }
            }
        }

        public static class Helpers
        {
            public class AccessPointV1
            {
                [Required]
                public string Bssid { get; set; }
                
                public string Ssid { get; set; }

                [Required]
                public double? Frequency { get; set; }

                [Required]
                public int? LowSignalLevel { get; set; }

                [Required]
                public double? LowSignalLatitude { get; set; }

                [Required]
                public double? LowSignalLongitude { get; set; }

                [Required]
                public int? HighSignalLevel { get; set; }

                [Required]
                public double? HighSignalLatitude { get; set; }

                [Required]
                public double? HighSignalLongitude { get; set; }

                [Required]
                public string RawSecurityPayload { get; set; }
            }
        }
    }
}
