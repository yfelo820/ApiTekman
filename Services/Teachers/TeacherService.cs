using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;

namespace Api.Services.Teachers
{
    public class TeacherService : ITeacherService
    {        
        private readonly ISchoolsRepository<Teacher> _teacher;
        private readonly ISchoolsRepository<TeacherGroup> _teacherGroup;
        private readonly IClaimsService _claims;

        public TeacherService(ISchoolsRepository<Teacher> teacher, ISchoolsRepository<TeacherGroup> teacherGroup, IClaimsService claims)
        {
            _teacher = teacher;
            _teacherGroup = teacherGroup;
            _claims = claims;
        }

        public async Task<Teacher> AddTeacher()
        {
            var name = _claims.GetName();
            var surnames = _claims.GetSurName();
            var email = _claims.GetUserName();
            var schoolId = _claims.GetSchoolId();

            var teachers = await _teacher.Find(b => b.Email == email);

            if (teachers.Any())
            {
                var teacher = teachers.First();
                teacher.LastLoggedIn = DateTime.Now;
                await _teacher.Update(teacher);
                return teacher;
            }

            var newTeacher = new Teacher
            {
                Name = name,
                Surnames = surnames,
                Email = email,
                SchoolId = schoolId,
                LastLoggedIn = DateTime.Now
            };

            await _teacher.Add(newTeacher);
            return newTeacher;
        }

        public async Task<List<TeacherAndHisGroupsDTO>> GetAllTeacherOfSchool()
        {
            var teachers = await _teacher.Find(b => b.SchoolId == _claims.GetSchoolId());
            var teachersIds = teachers.Select(teacher => teacher.Id);
            var teachersGroups = await _teacherGroup.Find(teacherGroup => 
                teachersIds.Contains(teacherGroup.TeacherId) 
                && teacherGroup.Group.SubjectKey == _claims.GetSubjectKey()
                && teacherGroup.Group.LanguageKey == _claims.GetLanguageKey());

            return teachers.Select(teacher =>
            {
                var groupIds = teachersGroups.Where(teacherGroup => teacherGroup.TeacherId == teacher.Id)
                    .Select(teachersGroup => teachersGroup.GroupId).ToList();
                return new TeacherAndHisGroupsDTO
                {
                    TeacherId = teacher.Id,
                    Name = teacher.Name,
                    Surnames = teacher.Surnames,
                    Email = teacher.Email,
                    SchoolId = teacher.SchoolId,
                    groupList = groupIds
                }; 
            }).ToList();
        }
    }
}
