using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.DTO.Shared
{
	public class StudentLoginDTO {
		[Required]
		public string UserName { get; set; }
		[Required]
		public int AccessNumber { get; set; }
		[Required]
		public string GroupKey { get; set; }
		[Required]
		public string SubjectKey { get; set; }
	}
}