using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Api.Interfaces.Shared;
using Api.Interfaces.Teachers;

namespace Api.Services.Teachers
{
    public class TeacherGroupService :ITeacherGroupService
    {
        private readonly ISchoolsRepository<Group> _groups;
        private readonly ISchoolsRepository<Teacher> _teachers;
        private readonly ISchoolsRepository<TeacherGroup> _teacherGroups;
        private readonly IHttpContextService _httpContextService;
        private readonly IClaimsService _claims;

        public TeacherGroupService
            (ISchoolsRepository<Group> groups, ISchoolsRepository<TeacherGroup> teacherGroups, 
             IHttpContextService httpContextService, IClaimsService claims,
             ISchoolsRepository<Teacher> teachers
            )
        {
            _groups = groups;
            _teacherGroups = teacherGroups;
            _httpContextService = httpContextService;
            _claims = claims;
            _teachers = teachers;
        }

        public async Task<List<TeacherGroup>> AddTeacherToGroup(Guid groupId, List<Guid> teacherIds)
        {
            var listTeacherGroup = new List<TeacherGroup>();
            foreach (var tchId in teacherIds)
            {
                var teacherInGroup = (await _teacherGroups.Find(b => b.GroupId == groupId && b.TeacherId == tchId));
                var checkNotExist = teacherInGroup.Count;

                if (checkNotExist > 0) 
                { 
                   listTeacherGroup.Add(teacherInGroup.SingleOrDefault());
                }
                else
                {
                    var teacherGroup = new TeacherGroup
                    {
                        GroupId = groupId,
                        TeacherId = tchId
                    };

                    await _teacherGroups.Add(teacherGroup);
                    listTeacherGroup.Add(teacherGroup);
                }
            }
            return listTeacherGroup;
        }

        public async Task<List<Group>> GetGroupsOfTeacher()
        {
            var email = _claims.GetUserName();
            var teacherId = (await _teachers.Find(b => b.Email == email)).Select(b => b.Id).FirstOrDefault();

            if (teacherId != default)
            {
                var subjectKey = _httpContextService.GetSubjectFromUri();
                var listGroupsIds = (await _teacherGroups.Find(b => b.TeacherId == teacherId)).Select(g => g.GroupId).ToList();
                var groupsList = await _groups.Find((b) => listGroupsIds.Contains(b.Id) && b.SubjectKey == subjectKey);

                return groupsList;
            }

            return new List<Group> { };            
        }
    }
}
