using System;
using Api.Entities.Content;


namespace Api.DTO.Backoffice
{
    public class ActivityDTO
    {
        public Guid Id { get; set; }

        public virtual Course Course { get; set; }

        public virtual Entities.Content.Subject Subject { get; set; }

        public virtual Language Language { get; set; }

        public virtual ContentBlock ContentBlock { get; set; }

        public int Session { get; set; }

        public int Difficulty { get; set; }

        public int Stage { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public bool IsTimeDependant { get; set; }

        public bool IsDiagnosis { get; set; }

        public int WordCount { get; set; }

        public int QuestionCount { get; set; }

        public ProblemResolutionDTO ProblemResolution { get; set; }
    }
}