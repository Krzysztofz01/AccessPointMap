using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Domain.AccessPoints;
using System;
using System.Collections.Generic;

namespace AccessPointMap.Application.AccessPoints
{
    public static class Commands
    {
        public static class V1
        {
            public class Create : IApplicationCommand<AccessPoint>
            {
                public IEnumerable<Helpers.AccessPointV1> AccessPoints { get; set; }

                public DateTime? ScanDate { get; set; }
            }

            public class Delete : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }
            }

            public class DeleteRange : IApplicationCommand<AccessPoint>
            {
                public IEnumerable<Guid> Ids { get; set; }
            }

            public class Update : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }

                public string Note { get; set; }
            }

            public class ChangeDisplayStatus : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }

                public bool? Status { get; set; }
            }

            public class ChangeDisplayStatusRange : IApplicationCommand<AccessPoint>
            {
                public IEnumerable<Guid> Ids { get; set; }

                public bool? Status { get; set; }
            }

            public class MergeWithStamp : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }

                public Guid StampId { get; set; }

                public bool? MergeLowSignalLevel { get; set; }
                
                public bool? MergeHighSignalLevel { get; set; }
                
                public bool? MergeSsid { get; set; }
                
                public bool? MergeSecurityData { get; set; }
            }

            public class DeleteStamp : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }

                public Guid StampId { get; set; }
            }

            public class CreateAdnnotation : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }

                public string Title { get; set; }

                public string Content { get; set; }
            }
            
            public class DeleteAdnnotation : IApplicationCommand<AccessPoint>
            {
                public Guid Id { get; set; }

                public Guid AdnnotationId { get; set; }
            }
        }

        public static class Helpers
        {
            public class AccessPointV1
            {
                public string Bssid { get; set; }
                
                public string Ssid { get; set; }

                public double? Frequency { get; set; }

                public int? LowSignalLevel { get; set; }

                public double? LowSignalLatitude { get; set; }

                public double? LowSignalLongitude { get; set; }

                public int? HighSignalLevel { get; set; }

                public double? HighSignalLatitude { get; set; }

                public double? HighSignalLongitude { get; set; }

                public string RawSecurityPayload { get; set; }
            }
        }
    }
}
