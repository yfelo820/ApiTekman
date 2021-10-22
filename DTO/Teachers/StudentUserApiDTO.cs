using System;
using Newtonsoft.Json;

namespace Api.DTO.Teachers
{
	// This class represents a sudent from the userApi
	public class StudentUserApiDTO {

		public string UserName { get; set; }

		public string Name { get; set; }
		public string FirstSurname { get; set; }
		public string SecondSurname { get; set; }
		public string Photo { get; set; }
		public string Email { get; set; }
		public string Stage { get; set; }
		public bool IsActive {get; set; }

		public int Course { get; set; }

		public string Password { get; set; }

		public StudentUserApiDTO() {}

		public StudentUserApiDTO(StudentDTO student) {
			if (student == null) return;
			
			UserName = student.UserName;
			Name = student.Name;
			FirstSurname = student.FirstSurname;
			SecondSurname = student.SecondSurname;
			Photo = student.Photo;
			Email = student.Email;
			Course = student.Course;
		}
	}
}