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
        private static readonly HttpClient Client = new HttpClient();
        private readonly string ApiBaseUrl = "https://api.macvendors.com/";

        public async Task<string> GetByBssid(string bssid)
        {
            try
            {
                string response = await Client.GetStringAsync(ApiBaseUrl + HttpUtility.UrlEncode(bssid));
                if (response.Contains("error", StringComparison.OrdinalIgnoreCase))
                {
                    return "No brand info";
                }
                else
                {
                    return response;
                }
            }
            catch (HttpRequestException)
            {
                return "No brand info";
            }
        }
    }
}
