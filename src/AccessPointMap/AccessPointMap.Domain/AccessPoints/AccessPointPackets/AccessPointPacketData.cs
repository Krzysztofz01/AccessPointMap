using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.AccessPoints.AccessPointPackets
{
    public class AccessPointPacketData : ValueObject<AccessPointPacketData>
    {
        public string Value { get; private set; }

        private AccessPointPacketData() { }
        private AccessPointPacketData(string value)
        {
            if (value.IsEmpty())
                throw new ValueObjectValidationException("The access point packet encoded data buffer can not be empty.");

            Span<byte> buffer = new Span<byte>(new byte[value.Length]);
            if (!Convert.TryFromBase64String(value, buffer, out var _))
                throw new ValueObjectValidationException("The access point packet data must be Base64 encoded.");

            Value = value.ToUpper();
        }

        public static implicit operator string(AccessPointPacketData data) => data.Value;
        public static implicit operator byte[](AccessPointPacketData data) => Convert.FromBase64String(data.Value);

        public static AccessPointPacketData FromString(string data) => new AccessPointPacketData(data);
        public static AccessPointPacketData FromBuffer(byte[] data) => new AccessPointPacketData(Convert.ToBase64String(data));
    }
}
