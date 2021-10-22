using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
    public class ProblemResolution : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public Guid LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}
