using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System.Net.Mail;

namespace AccessPointMap.Domain.Identities
{
    public class IdentityEmail : ValueObject<IdentityEmail>
    {
        public string Value { get; private set; }

        private IdentityEmail() { }
        private IdentityEmail(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationException("The identity email length is invalid.");

            if (!MailAddress.TryCreate(value, out var _))
                throw new ValueObjectValidationException("The identity email format is invalid.");

            Value = value;
        }

        public static implicit operator string(IdentityEmail email) => email.Value;

        public static IdentityEmail FromString(string email) => new IdentityEmail(email);
    }
}
