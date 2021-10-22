using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Entities.Schools
{
    public class ParentFeedbackAnswerSet : BaseEntity
    {
        public Guid QuestionSetId { get; set; }
        public DateTime FulfillmentDate { get; set; }
        public string ParentEmail { get; set; }
        public string UserName { get; set; }
        public string QuestionText1 { get; set; }
        public string QuestionText2 { get; set; }
        public string QuestionText3 { get; set; }
        public string QuestionText4 { get; set; }
        public string QuestionText5 { get; set; }
        public int AnswerValue1 { get; set; }
        public int AnswerValue2 { get; set; }
        public int AnswerValue3 { get; set; }
        public int AnswerValue4 { get; set; }
        public int AnswerValue5 { get; set; }
        public string Comments { get; set; }
    }
}
