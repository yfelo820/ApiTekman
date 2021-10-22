using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO.Teachers
{
    public class TeacherAndHisGroupsDTO
    {
        public Guid TeacherId { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string SchoolId { get; set; }
        public List<Guid> groupList { get; set; }
    }
}
