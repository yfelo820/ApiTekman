using System;
using System.Collections.Generic;
using Api.DTO.Teachers;
using Newtonsoft.Json;

namespace Api.DTO.Parents
{
	public class StudentUserApiDTO {
        public string SchoolId { get; set; }
		public string Stage { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
		public string FirstSurname { get; set; }
        public DateTime BirthDate { get; set; }
        public string SecondSurname { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Photo { get; set; }
		public string Password { get; set; }
        public int AccessNumber { get; set; }
        public string GroupKey { get; set; }
        public string SubjectKey { get; set; }
        public string LanguageKey { get; set; }
		public StudentUserApiDTO() {}
	}

    public class StudentsUserApiDTO
    {
        public IEnumerable<StudentUserApiDTO> Students { get; set; }
    }
}