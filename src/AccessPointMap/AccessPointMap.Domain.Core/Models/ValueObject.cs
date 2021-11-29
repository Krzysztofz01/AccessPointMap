using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AccessPointMap.Domain.Core.Models
{
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        private IEnumerable<PropertyInfo> _properties;
        private IEnumerable<FieldInfo> _fields;

        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (Equals(a, null))
            {
                if (Equals(b, null))
                {
                    return true;
                }

                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }

        public bool Equals(ValueObject<T> obj)
        {
            return Equals(obj as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool FieldsAreEqual(object obj, FieldInfo f)
        {
            return Equals(f.GetValue(this), f.GetValue(obj));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p)
        {
            return Equals(p.GetValue(this, null), p.GetValue(obj, null));
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (_fields == null)
            {
                _fields = GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .ToList();
            }

            return _fields;
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (_properties == null)
            {
                _properties = GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToList();
            }

            return _properties;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
