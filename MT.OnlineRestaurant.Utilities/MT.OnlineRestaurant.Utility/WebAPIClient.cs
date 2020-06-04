using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MT.OnlineRestaurant.Utility
{
    public class WebAPIClient
    {
        public const string TokenKey = "AuthToken";
        public const string UserKey = "CustomerId";
        public static HttpClient GetClient(string token, int customerId, string url)
        {
            HttpClient client = new HttpClient();
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;

            client = new HttpClient(handler);
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add(TokenKey, token);
            client.DefaultRequestHeaders.Add(UserKey, Convert.ToString(customerId));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
