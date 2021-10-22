using Api.Exceptions;
using Api.Interfaces.Shared;
using Api.Settings;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.Services.Shared
{
    public class RecaptchaService : IRecaptchaService
    {
        private readonly RecaptchaSettings _settings;

        public RecaptchaService(IOptions<RecaptchaSettings> options)
        {
            _settings = options.Value;
        }

        public async Task Validate(string token)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _settings.PrivateKey),
                    new KeyValuePair<string, string>("response", token)
                });
                var captchaResult = await client.PostAsync(_settings.VerificationUrl, content);
                string captchaResultContent = await captchaResult.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<RecaptchaValidationResponse>(captchaResultContent);
                if (!response.Success)
                {
                    throw new CaptchaValidationException(response.Errors.Join("\n"));
                }
            }
        }
    }

    public class RecaptchaValidationResponse
    {
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public IEnumerable<string> Errors { get; set; }
    }
}
