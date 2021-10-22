using Newtonsoft.Json;

namespace Api.DTO.Shared
{
	public class TokenRequestDto
	{
		public string grant_type { get; set; }
		public string client_id { get; set; }
		public string client_secret { get; set; }
		public string redirect_uri { get; set; }
		public string code { get; set; }

		public TokenRequestDto(string loginCode, ServiceProvider config, string redirectUri)
		{
			grant_type = "authorization_code";
			client_id = config.ClientId;
			client_secret = config.ClientSecret;
			redirect_uri = redirectUri;
			code = loginCode;
		}
	}

	public class TokenResponseDto
	{
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
		[JsonProperty("scope")] 
		public string Scope { get; set; }
		[JsonProperty("id_token")]
		public string IdToken { get; set; }
		[JsonProperty("token_type")]
		public string TokenType { get; set; }
		[JsonProperty("expires_in")]
		public string ExpiresIn { get; set; }
	}

	public class SsoClaimsDTO
	{
		public string sub { get; set; }
		public int school_id { get; set; }
		public string roles { get; set; }
		public string school_name { get; set; }
		public string user_api_token { get; set; }
		public string given_name { get; set; }
		public string locale { get; set; }
		public string family_name { get; set; }
		public string email { get; set; }
		public string picture { get; set; }
		public string sid { get; set; }
	}
	public class ServiceProvider
	{
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
	}
}