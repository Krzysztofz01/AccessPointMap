using System;
using System.Collections.Generic;

namespace AccessPointMap.Service.Dto
{
    public class AccessPointStatisticsDto
    {
        public int AllRecords { get; set; }
        public int InsecureRecords { get; set; }
        public IEnumerable<Tuple<string, int>> TopBrands { get; set; }
        public IEnumerable<AccessPointDto> TopAreaAccessPoints { get; set; }
        public IEnumerable<Tuple<string, int>> TopSecurityTypes { get; set; }
        public IEnumerable<Tuple<double, int>> TopFrequencies { get; set; }
    }
}
