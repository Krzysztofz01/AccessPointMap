using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Web.ViewModels
{
    public class AccessPointStatisticsGetView
    {
        public int AllRecords { get; set; }
        public int InsecureRecords { get; set; }
        public IEnumerable<string> TopBrands { get; set; }
        public IEnumerable<AccessPointGetView> TopAreaAccessPoints { get; set; }
        public IEnumerable<string> TopSecurityTypes { get; set; }
        public IEnumerable<double> TopFrequencies { get; set; }
    }
}
