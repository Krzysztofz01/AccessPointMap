using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Domain.AccessPoints.AccessPointAdnnotations
{
    public class AccessPointAdnnotationTitle : ValueObject<AccessPointAdnnotationTitle>
    {
        public string Value { get; private set; }

        private AccessPointAdnnotationTitle() { }
        private AccessPointAdnnotationTitle(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationException("Invalid access point adnnotation title.");

            Value = value;
        }

        public static implicit operator string(AccessPointAdnnotationTitle title) => title.Value;

        public static AccessPointAdnnotationTitle FromString(string title) => new AccessPointAdnnotationTitle(title);
    }
}
