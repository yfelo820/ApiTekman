using System;

namespace Api.Entities.Content
{
    public class Multimedia : BaseEntity
    {
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public Guid LanguageId { get; set; }
        public virtual Language Language { get; set; }
        public string Title { get; set; }
        public MediaType Type { get; set; }
        public string FileName { get; set; }
    }
}