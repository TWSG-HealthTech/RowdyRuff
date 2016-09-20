using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RowdyRuff.Common.Gateway
{
    public class ServerGatewayBase : IServerGateway
    {
        public Task<T> GetAsyncWithBasicAuth<T>(string path, string username, string password)
        {
            var byteArray = Encoding.ASCII.GetBytes(username + ":" + password);
            string auth = Convert.ToBase64String(byteArray);
            return GetAsync<T>(path, auth);
        }

        public Task<T> GetAsync<T>(string path, string auth)
        {
            return Execute<T>(path, auth);
        }

        private static async Task<T> Execute<T>(string path, string auth)
        {
            using (var httpClient = new HttpClient())
            {
                if (!string.IsNullOrEmpty(auth))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
                }
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.GetAsync(path);

                //will throw an exception if not successful
                response.EnsureSuccessStatusCode();

                return await DeserializeResponse<T>(response);
            }
        }

        private static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
