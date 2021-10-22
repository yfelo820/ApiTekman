using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Api.Helpers
{
	public class HttpFormClient: BaseHttpClient
	{
		public HttpFormClient(string baseUrl) : base(baseUrl) {}

		public async Task<T> Post<T>(string url, object value)
		{
			var formData = new List<KeyValuePair<string, string>>();
			// Building a list of key, values with the object properties
			foreach (PropertyInfo prop in value.GetType().GetProperties()) {
				formData.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(value).ToString()));
			}
			// Sending the key-value list as FormUrlEncoded
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content = new FormUrlEncodedContent(formData);
			var response = await _client.SendAsync(request);
			return await ParseResponse<T>(response);
		}

		public string GetRequestUrl(string url, object value)
		{
			var formData = new List<KeyValuePair<string, string>>();
			// Building a list of key, values with the object properties
			foreach (PropertyInfo prop in value.GetType().GetProperties()) {
				formData.Add(new KeyValuePair<string, string>(prop.Name, prop.GetValue(value).ToString()));
			}
			// Sending the key-value list as FormUrlEncoded
			var request = new HttpRequestMessage(HttpMethod.Post, url);
			request.Content = new FormUrlEncodedContent(formData);
			return request.RequestUri.AbsoluteUri.ToString();
		}

		public async Task<T> Get<T>(string url)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, url);
			var response = await _client.SendAsync(request);
			return await ParseResponse<T>(response);
		}

		public void SetAuth(string token) 
		{
			if (string.IsNullOrEmpty(token)) return;
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}
	}
}
