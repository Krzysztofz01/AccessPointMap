using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AccessPointMapWebApi.Repositories
{
    public interface IBrandRepository
    {
        Task<string> GetByBssid(string bssid);
    }

    public class BrandRepository : IBrandRepository
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string ApiBaseUrl = "https://api.macvendors.com/";

        public BrandRepository(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetByBssid(string bssid)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{ ApiBaseUrl }{ HttpUtility.UrlEncode(bssid) }");
            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                if(responseString.Contains("error", StringComparison.OrdinalIgnoreCase) || responseString.Length == 0)
                {
                    return "No brand info";
                }
                return responseString.Trim();
                
                
            }
            return "No brand info";
        }
    }
}
