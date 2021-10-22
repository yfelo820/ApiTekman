using System;
using System.Collections.Generic;
using System.Linq;
using Api.Entities.Schools;

namespace Api.DTO.Teachers
{
	public class GroupDTO {

		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Key { get; set; }
		public int Course { get; set; }		
		public List<string> StudentUserNames { get; set; }
		public bool FirstSessionWithDiagnosis { get; set; }
		public bool AccessAllSessions { get; set; }
		public int LimitSession { get; set; }
		public int LimitCourse { get; set; }
        public bool AccessAllCourses { get; set; }
        public bool AccessFromHome { get; set; }
        public bool ParentRating { get; set; }


        public GroupDTO() {}
		public GroupDTO(Group group)
		{
			Id = group.Id;
			Name = group.Name;
			Key = group.Key;
			Course = group.Course;
			StudentUserNames = group.Students.Select( s => s.UserName ).ToList();
			FirstSessionWithDiagnosis = group.FirstSessionWithDiagnosis;
			AccessAllSessions = group.AccessAllSessions;
			LimitSession = group.LimitSession;
			LimitCourse = group.LimitCourse;
            AccessAllCourses = group.AccessAllCourses;
            AccessFromHome = group.AccessFromHome;
            ParentRating = group.ParentRating;
        }
	}
}