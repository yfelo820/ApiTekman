using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
    public class SuperContentBlock : BaseEntity
    {
        [Range(1, int.MaxValue)]
        public int Order { get; set; }
        
        public string Name { get; set; }
        
        public Guid LanguageId { get; set; }
        public virtual Language Language { get; set; }
        
        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}