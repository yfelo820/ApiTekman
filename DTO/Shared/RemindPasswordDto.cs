using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Shared
{
	public class RemindPasswordDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}