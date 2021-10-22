using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Shared
{
    public class LoginDTO
    {
		[Required]
        [EmailAddress]
		public string Email { get; set; }

		[Required]
        [DataType(DataType.Password)]
		public string Password { get; set; }

		public bool RememberMe { get; set; }
    }
}