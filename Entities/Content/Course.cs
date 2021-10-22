using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
	public class Course : BaseEntity
	{
		[Required]
		public int Number { get; set; }
	}
}