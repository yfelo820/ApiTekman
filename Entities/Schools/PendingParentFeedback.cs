using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities.Schools
{
    public class PendingParentFeedback : BaseEntity
    {
        public string UserName { get; set; }
        public DateTime RequestTime { get; set; }
        public bool IsRead { get; set; }
    }
}
