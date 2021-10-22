using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Shared
{
    public class SignUpConfirmationDto
    {
		[Required]
        [EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Token { get; set; }
    }
}