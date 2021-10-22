using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO.Teachers
{
    public class ParentFeedbackAverageValuationDTO
    {
        public string StudentUserName { get; set; }
        public int AverageValue { get; set; }
        public int CommentCount { get; set; }
    }
}
