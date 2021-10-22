using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
	public class ContentBlock : BaseEntity
	{
		[Range(1, int.MaxValue)]
		public int Order { get; set; }
		public string Name { get; set; }
		public string Image { get; set; }

		public Guid LanguageId { get; set; }
		public virtual Language Language { get; set; }

		public Guid SubjectId { get; set; }
		public virtual Subject Subject { get; set; }

        public Guid? SuperContentBlockId { get; set; }
        public virtual SuperContentBlock SuperContentBlock { get; set; }
    }
}