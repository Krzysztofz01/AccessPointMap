using System;

namespace AccessPointMap.Domain.Common
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime EditDate { get; set; }
        protected DateTime? DeleteDate { get; set; }

        public abstract void Validate();

        public void SetCreated() => AddDate = DateTime.Now;
        public void SetModified() => EditDate = DateTime.Now;
        public void Delete() => DeleteDate = DateTime.Now;
        public bool IsDeleted() => DeleteDate == null;
    }
}
