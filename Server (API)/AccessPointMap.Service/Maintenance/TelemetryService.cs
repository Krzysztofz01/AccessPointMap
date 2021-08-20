using AccessPointMap.Service.Maintenance.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Maintenance
{
    public class TelemetryService : ITelemetryService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _telemetryHeader = "apmtlm";
        private readonly Uri _telemetryServerUri = new Uri("");

        public TelemetryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task LogEvent(Report report)
        {            
            var client = _httpClientFactory.CreateClient();

            var content = new StringContent(JsonConvert.SerializeObject(report), Encoding.UTF8, "application/json");

            content.Headers.Add(_telemetryHeader, DateTime.Now.ToString());

            await client.PostAsync(_telemetryServerUri, content);
        }

        public async Task Ping()
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, _telemetryServerUri);

            request.Headers.Add(_telemetryHeader, DateTime.Now.ToString());

            await client.SendAsync(request);
        }
    }
}
