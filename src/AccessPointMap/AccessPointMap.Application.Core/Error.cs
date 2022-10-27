using System;

namespace AccessPointMap.Application.Core
{
    public class Error
    {
        public string Message { get; protected init; }

        protected Error() { }

        protected Error(string message)
        {
            Message = string.IsNullOrWhiteSpace(message)
                ? string.Empty : message.Trim();
        }

        public static implicit operator string(Error error) => error.Message;

        public static Error Default => new("Opeartion failed.");
        public static Error FromException(Exception exception) => new(exception.Message);
        public static Error FromString(string messageValue) => new(messageValue);
    }
}
