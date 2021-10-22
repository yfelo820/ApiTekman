using System;

namespace Api.DTO.Students
{
    public class StudentProblemsAnswerDTO
    {
        public Guid ActivityContentBlockId { get; set; }

        public string SubjectKey { get; set; }

        public int Course { get; set; }
        public int Session { get; set; }
        public int Stage { get; set; }

        public string UserName { get; set; }
    }
}