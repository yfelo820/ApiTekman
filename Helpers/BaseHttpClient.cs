using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Api.Exceptions;
using Newtonsoft.Json;

namespace Api.Helpers
{
    public abstract class BaseHttpClient
    {
        protected HttpClient _client = new HttpClient();

        protected BaseHttpClient(string baseUrl)
        {
            BaseAddress = baseUrl;
        }

        public string BaseAddress
        {
            set
            {
                _client.BaseAddress = new System.Uri(value);
            }
        }

        protected async Task<T> ParseResponse<T>(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest || 
                    response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new HttpException(response.StatusCode, responseString);
                }

                throw new TKCoreException($"Something was wrong at TKCore: StatusCode = {(int)response.StatusCode}, Response = {responseString}");
            }

            return typeof(T) == typeof(string) ?
                (T)(object)responseString :
                JsonConvert.DeserializeObject<T>(responseString);
        }
    }
}