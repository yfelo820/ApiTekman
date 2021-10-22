using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Helpers
{
	public class HttpJsonClient: BaseHttpClient
	{

		public HttpJsonClient(string baseUrl) : base(baseUrl) {}

		public async Task<T> Post<T>(string url, object value)
		{
			// Fixme: for some reason, the method PostAsJsonAsync does not work, and we have to do this
			// workaround
			// var response = await _client.PostAsJsonAsync(url, value)
			var json = ToJsonContent(value);
			var response = await _client.PostAsync(url, json);
			// ///
			return await ParseResponse<T>(response);
		}

		public async Task<T> Get<T>(string url)
		{
			var response = await _client.GetAsync(url);
			return await ParseResponse<T>(response);
		}

		public void SetAuth(string token) 
		{
			if (string.IsNullOrEmpty(token)) return;
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

		private ByteArrayContent ToJsonContent(object value) {
			var stringValue = JsonConvert.SerializeObject(value);
			var buffer = System.Text.Encoding.UTF8.GetBytes(stringValue);
			var byteValue = new ByteArrayContent(buffer);
			byteValue.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return byteValue;
		}
	}
}
