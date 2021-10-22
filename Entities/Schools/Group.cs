using System;
using System.Collections.Generic;
using Api.DTO.Teachers;


namespace Api.Entities.Schools
{
    public class Group : BaseEntity
	{
        public Guid? TkGroupId { get; set; }
        public string Key { get; set; }

		public string SubjectKey { get; set; }

		public string Name { get; set; }

		// Both students and groups have a Course. The course of the group always "win" over the
		// student's one. Meaning that, if a student with course 2 is set in a group of course 1, 
		// we must SET the student course to 1. If a group changes its course to 3, 
		// we must change all of the students courses to 3.
		public int Course { get; set; }
		
		public string SchoolId { get; set; }

		public string LanguageKey { get; set; }
		
		public bool FirstSessionWithDiagnosis { get; set; }
		public bool AccessAllSessions { get; set; }
		public int LimitSession { get; set; }
		public int LimitCourse { get; set; }

        public virtual List<StudentGroup> Students { get; set; }

        public bool AccessAllCourses { get; set; }
        public bool AccessFromHome { get; set; }
        public bool ParentRating { get; set; }

        public Group() {}
		public Group(GroupDTO groupDTO)
		{
			Name = groupDTO.Name;
			Course = groupDTO.Course;
			Students = new List<StudentGroup>();
			LimitCourse = groupDTO.LimitCourse;
			LimitSession = groupDTO.LimitSession;
			AccessAllSessions = groupDTO.AccessAllSessions;
			FirstSessionWithDiagnosis = groupDTO.FirstSessionWithDiagnosis;
            AccessAllCourses = groupDTO.AccessAllCourses;
            AccessFromHome = groupDTO.AccessFromHome;
            ParentRating = groupDTO.ParentRating;
        }
	}
}