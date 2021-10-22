using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
	public class Feedback : Scene
	{
		public Guid SubjectId { get; set; }
		public virtual Subject Subject { get; set; }

		public Guid LanguageId { get; set; }
		public virtual Language Language { get; set; }
		
		[Range(0, 4)]
		public int Score { get; set; }
	}
}