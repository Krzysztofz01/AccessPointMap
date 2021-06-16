using Microsoft.AspNetCore.Http;

namespace AccessPointMap.Web.Utilities
{
    public static class AddressUtility
    {
        public static string GetIpV4(HttpRequest request)
        {
            string ipFormHeader = request.HttpContext.Connection.RemoteIpAddress.ToString();
            return (!string.IsNullOrWhiteSpace(ipFormHeader)) ? ipFormHeader : string.Empty;
        }
    }
}
