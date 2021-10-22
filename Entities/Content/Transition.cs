using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace Api.Entities.Content
{
	public class Transition : BaseEntity
	{
		[Required]
		public string Name { get; set; }

		public virtual List<TransitionProperty> TransitionProperties {get; set; }
	}
}