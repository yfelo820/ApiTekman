using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Api.DTO.Teachers;


namespace Api.Entities.Schools
{
	public class SsoIdentity : BaseEntity
	{
		public string Email { get; set; }
		public string IdToken { get; set; }
	}
}