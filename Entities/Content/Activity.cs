using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities.Content
{
	public class Activity : BaseEntity
	{
		public Guid CourseId { get; set; }
		public virtual Course Course { get; set; }

		public Guid SubjectId { get; set; }
		public virtual Subject Subject { get; set; }

		public Guid LanguageId { get; set; }
		public virtual Language Language { get; set; }

		public Guid? ContentBlockId { get; set; }
		public virtual ContentBlock ContentBlock { get; set; }

		[Range(1, Int32.MaxValue)]
		public int Session { get; set; }
		
		[Range(1, Int32.MaxValue)]
		public int Difficulty { get; set; }

		public int Stage { get; set; }
		
		[StringLength(150)]
		public string Description { get; set; }

		[StringLength(50)]
		public string ShortDescription { get; set; } 
		
		// If true this activity will be a test, will be blocked from all students until the teacher
		// activates it and will be evaluated with a diferent rubric.
		public bool IsTimeDependant { get; set; }
		
		// If true this activity will be part of the diagnosis test for that course
		public bool IsDiagnosis { get; set; }

		// This two vars are only relevant if IsTimeDependant = true
		public int WordCount { get; set; }
		public int QuestionCount { get; set; }	

		public virtual List<Exercise> Exercises  { get; set; }

		public bool IsValid
		{
			get
			{
				if (Subject == null) return false;
				return Difficulty <= Subject.DifficultyCount && Session <= Subject.SessionCount;
			}
		}

        public Guid? ProblemResolutionId { get; set; }

        public virtual ProblemResolution ProblemResolution { get; set; }
    }
}