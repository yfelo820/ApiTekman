using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities.Schools
{
    public class TeacherGroup : BaseEntity
    {        
        public Guid TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
