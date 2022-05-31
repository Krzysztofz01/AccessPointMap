using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations
{
    public class AccessPointAdnnotationContent : ValueObject<AccessPointAdnnotationContent>
    {
        public string Value { get; private set; }

        private AccessPointAdnnotationContent() { }
        private AccessPointAdnnotationContent(string value)
        {
            if (value is null)
                throw new ValueObjectValidationException("Invalid access point adnnotation content.");

            Value = value;
        }

        public static implicit operator string(AccessPointAdnnotationContent content) => content.Value;

        public static AccessPointAdnnotationContent FromString(string content) => new AccessPointAdnnotationContent(content);
        public static AccessPointAdnnotationContent Empty => new AccessPointAdnnotationContent(string.Empty);
    }
}
