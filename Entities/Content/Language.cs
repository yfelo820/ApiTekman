using System.ComponentModel.DataAnnotations;


namespace Api.Entities.Content
{
	public class Language : BaseEntity
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Code { get; set; }
	}
}