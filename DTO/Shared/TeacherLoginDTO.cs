using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Shared
{
    public class TeacherLoginDTO
    {
		[Required]
        [EmailAddress]
		public string Email { get; set; }

		[Required]
        [DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		public string SubjectKey { get; set; }
    }
}