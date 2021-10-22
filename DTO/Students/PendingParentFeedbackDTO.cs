using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO.Students
{
    public class PendingParentFeedbackDTO
    {
        public string UserName { get; set; }
        public DateTime RequestTime { get; set; }
        public bool IsRead { get; set; }
    }

    public class PendingParentFeedbackDTOExtended : PendingParentFeedbackDTO
    {
        public string Name { get; set; }
    }
}
