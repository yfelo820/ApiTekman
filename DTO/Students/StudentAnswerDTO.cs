using System;

namespace Api.DTO.Students
{
    public class StudentAnswerDTO
    {
        public Guid ActivityId { get; set; }
        public int Session { get; set; }
        public int Stage { get; set; }
        public float Grade { get; set; }
        public float StudentGrade { get; set; }
    }
}