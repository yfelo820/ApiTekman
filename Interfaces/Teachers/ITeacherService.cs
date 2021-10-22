using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Teachers;
using Api.Entities.Schools;

namespace Api.Interfaces.Teachers
{
    public interface ITeacherService  
    {
        Task<Teacher> AddTeacher();
        Task<List<TeacherAndHisGroupsDTO>> GetAllTeacherOfSchool();
    }
}
