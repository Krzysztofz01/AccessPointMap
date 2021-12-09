using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointNote : ValueObject<AccessPointNote>
    {
        public string Value { get; private set; }

        private AccessPointNote() { }
        private AccessPointNote(string value)
        {
            Value = value;
        }

        public static implicit operator string(AccessPointNote note) => note.Value;

        public static AccessPointNote FromString(string note) => new AccessPointNote(note);
        public static AccessPointNote Empty => new AccessPointNote(string.Empty);
    }
}
