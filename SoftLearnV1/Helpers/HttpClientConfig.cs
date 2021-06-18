using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SoftLearnV1.Helpers
{
    public class HttpClientConfig
    {
        //------------------------------------------USE THIS METHOD FOR GET REQUESTS ----------------------
        public async Task<string> ApiGetRequest(string url, string tokenOrKey)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenOrKey);
                string apiResponse = await httpClient.GetStringAsync(url);

                return apiResponse;
            }
        }

        public async Task<string> ApiPostRequest(string url, object obj, string tokenOrKey)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenOrKey);
                var response = await httpClient.PostAsync(url, content);
                string apiResponse = await response.Content.ReadAsStringAsync();

                return apiResponse;
            }
        }
    }
}
