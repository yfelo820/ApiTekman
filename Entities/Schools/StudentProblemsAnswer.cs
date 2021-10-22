using System;

namespace Api.Entities.Schools
{
    public class StudentProblemsAnswer : BaseEntity
    {
        public Guid ActivityContentBlockId { get; set; }

        public string SubjectKey { get; set; }
        
        public int Course { get; set; }
        public int Session { get; set; }
        public int Stage { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}