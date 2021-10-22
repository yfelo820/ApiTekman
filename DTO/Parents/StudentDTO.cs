using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO.Parents
{
    public class StudentDTO
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string FirstSurname { get; set; }
        public string SecondSurname { get; set; }
        public string Photo { get; set; }
        public string Email { get; set; }
        public string GroupKey { get; set; }
        public string SubjectKey { get; set; }
        public string LanguageKey { get; set; }

        public int AccessNumber { get; set; }

        public StudentDTO(StudentUserApiDTO student)
        {
            if (student == null) return;

            UserName = student.UserName;
            Name = student.Name;
            FirstSurname = student.FirstSurname;
            SecondSurname = student.SecondSurname;
            Photo = student.Photo;
            Email = student.Email;
            GroupKey = student.GroupKey;
            SubjectKey = student.SubjectKey;
            LanguageKey = student.LanguageKey;
            AccessNumber = student.AccessNumber;
        }
    }
}
