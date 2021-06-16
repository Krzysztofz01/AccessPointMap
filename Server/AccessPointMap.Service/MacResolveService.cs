using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AccessPointMap.Service
{
    public class MacResolveService : IMacResolveService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<MacResolveService> logger;

        private readonly Uri macApiUri = new Uri("https://api.macvendors.com/");

        public MacResolveService(
            IHttpClientFactory httpClientFactory,
            ILogger<MacResolveService> logger)
        {
            this.httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));

            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetVendorV1(string bssid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ macApiUri }{ HttpUtility.UrlEncode(bssid) }");
            var client = httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound) return null;

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                
                if(responseString.ToLower().Contains("error") || string.IsNullOrEmpty(responseString))
                {
                    return null;
                }

                return responseString.Trim();
            }

            //If check only for 200 and 404, all other will indicate a foreign server error or limit
            return "#ERROR";
        }
    }
}
