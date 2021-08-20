using System;

namespace AccessPointMap.Service.Maintenance.Models
{
    public class Report
    {
        public Exception Exception { get; private set; }
        public string Message { get; private set; }
        public DateTime Date { get; private set; }
        public string Host { get; private set; }

        private Report()
        {
        }

        public static class Factory
        {
            public static Report Create(string host, string message, Exception exception = null)
            {
                return new Report()
                {
                    Host = host,
                    Message = message,
                    Exception = (exception is null) ? null : exception
                };
            }
        }
    }
}
