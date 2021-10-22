using System;
using Api.Entities.Schools;
using Newtonsoft.Json;

namespace Api.DTO.Teachers
{
	// This class represents a sudent from the userApi
	public class StudentDTO {

		public string UserName { get; set; }

		public string Name { get; set; }
		public string FirstSurname { get; set; }
		public string SecondSurname { get; set; }
		public string Photo { get; set; }
		public string Email { get; set; }

		public int Course { get; set; }
		public Group Group { get; set; }

		public int AccessNumber { get; set; }

		public StudentDTO(StudentUserApiDTO student, StudentGroup group = null) 
		{
			if (student == null) return;

			UserName = student.UserName;
			Name = student.Name;
			FirstSurname = student.FirstSurname;
			SecondSurname = student.SecondSurname;
			Photo = student.Photo;
			Email = student.Email;
			Course = 0;
	
			if (group != null) {
				Course = group.Group.Course;
				Group = group.Group;
				AccessNumber = group.AccessNumber;
			}
		}
	}
}