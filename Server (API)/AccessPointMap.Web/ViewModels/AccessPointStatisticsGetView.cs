using System;
using System.Collections.Generic;

namespace AccessPointMap.Web.ViewModels
{
    public class AccessPointStatisticsGetView
    {
        public int AllRecords { get; set; }
        public int InsecureRecords { get; set; }
        public IEnumerable<Tuple<string, int>> TopBrands { get; set; }
        public IEnumerable<AccessPointGetViewPublic> TopAreaAccessPoints { get; set; }
        public IEnumerable<Tuple<string, int>> TopSecurityTypes { get; set; }
        public IEnumerable<Tuple<double, int>> TopFrequencies { get; set; }
    }
}
