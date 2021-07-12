using System;

namespace AccessPointMap.Domain
{
    public class BaseEntity
    {
        public long Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EditDate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
