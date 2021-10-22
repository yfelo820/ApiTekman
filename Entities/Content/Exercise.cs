using System;

namespace Api.Entities.Content
{
	public class Exercise : Scene
	{
		public Guid ActivityId { get; set; }
		public virtual Activity Activity { get; set; }
		
		public int Order { get; set; }
	}
}