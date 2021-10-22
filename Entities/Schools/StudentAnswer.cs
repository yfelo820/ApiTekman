using System;

namespace Api.Entities.Schools
{
    public class StudentAnswer : BaseEntity
    {
        public string LanguageKey { get; set; }
        public string SubjectKey { get; set; }
        
        public int Course { get; set; }
        public int Session { get; set; }
        public int Stage { get; set; }
        public Guid? ActivityId { get; set; }
        public int ActivitySession { get; set; }
        public Guid ActivityContentBlockId { get; set; }
        public int ActivityDifficulty { get; set; }

        public float Grade { get; set; }
        public float? StudentGrade { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }

        public bool IsSentToTkReports { get; set; }

        // Only for tkreports
        public LevelType Level { get; set; }
    }

    public enum LevelType
    {
        Advance,
        Practise,
        Revise,
        ReviseAgain
    }
}