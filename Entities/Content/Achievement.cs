using System;

namespace Api.Entities.Content
{
	public class Achievement : Scene
	{
		public Guid SubjectId { get; set; }
		public virtual Subject Subject { get; set; }
		public Guid LanguageId { get; set; }
		public virtual Language Language { get; set; }
		
		public string Name { get; set; }
		public int Session { get; set; }
		public string Image { get; set; }
	}
}