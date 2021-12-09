using System;

namespace AccessPointMap.Domain.Core.Models
{
    public abstract class AggregateRoot : Entity
    {
        public override bool Equals(object obj)
        {
            var compare = obj as AggregateRoot;

            if (ReferenceEquals(this, compare)) return true;
            if (compare is null) return false;

            return Id.Equals(compare.Id);
        }

        public static bool operator ==(AggregateRoot a, AggregateRoot b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(AggregateRoot a, AggregateRoot b)
        {
            return !(a == b);
        }

        public static explicit operator Guid(AggregateRoot entity) => entity.Id;

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"AggregateRoot Id: {Id}";
        }
    }
}
