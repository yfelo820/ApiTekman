using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities.Schools
{
    public class ParentFeedbackQuestionSet : BaseEntity
    {
        public string QuestionSetType { get; set; }
        public string QuestionLabel1 { get; set; }
        public string QuestionLabel2 { get; set; }
        public string QuestionLabel3 { get; set; }
        public string QuestionLabel4 { get; set; }
        public string QuestionLabel5 { get; set; }
    }
}
