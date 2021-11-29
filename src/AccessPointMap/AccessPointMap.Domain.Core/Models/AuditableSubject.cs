using System;

namespace AccessPointMap.Domain.Core.Models
{
    public abstract class AuditableSubject
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
