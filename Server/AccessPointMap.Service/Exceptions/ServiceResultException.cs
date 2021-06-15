using System;

namespace Balto.Service.Exceptions
{
    public class ServiceResultException : Exception
    {
        public ServiceResultException()
        {
        }

        public ServiceResultException(string message) : base(message)
        {
        }

        public ServiceResultException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}