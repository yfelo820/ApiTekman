using Newtonsoft.Json;

namespace Api.DTO.Shared
{
	// This class represents the response object received after a login to the userApi
	public class UserApiLoginDTO
	{
		[JsonProperty("access_token")]
		public string Token { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("surnames")]
		public string Surnames { get; set; }

		[JsonProperty("schoolId")]
		public string SchoolId { get; set; }

		[JsonProperty("schoolName")]
		public string SchoolName { get; set; }

		[JsonProperty("photo")]
		public string Photo { get; set; }

		[JsonProperty("userName")]
		public string UserName { get; set; }

		[JsonProperty("academicYears")]
		public string AcademicYears { get; set; }

		[JsonProperty("languages")]
		public string Languages { get; set; }
	}
}