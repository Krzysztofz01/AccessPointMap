using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.Identities
{
    public class IdentityLastLogin : ValueObject<IdentityLastLogin>
    {
        public string IpAddress { get; private set; }
        public DateTime Date { get; private set; }

        private IdentityLastLogin() { }
        private IdentityLastLogin(string ipAddress)
        {
            if (ipAddress.IsEmpty())
                throw new ValueObjectValidationException("The identity ip address length is invalid.");

            IpAddress = ipAddress;
            Date = DateTime.Now;
        }

        public static implicit operator string(IdentityLastLogin lastLogin) => lastLogin.IpAddress;
        public static implicit operator DateTime(IdentityLastLogin lastLogin) => lastLogin.Date;

        public static IdentityLastLogin FromString(string ipAddress) => new IdentityLastLogin(ipAddress);
    }
}
