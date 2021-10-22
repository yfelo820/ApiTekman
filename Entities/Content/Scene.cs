using System;
using System.Collections.Generic;


namespace Api.Entities.Content
{
	public abstract class Scene : BaseEntity
	{
		public string Thumbnail { get; set; }
		public string BackgroundImage { get; set; }

		public Guid? TransitionId { get; set; }
		public virtual Transition Transition { get; set; }
		
		public virtual List<Item> Items  { get; set; }
	}
}