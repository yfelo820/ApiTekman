using System.Collections.Generic;

namespace Api.APIsMailing.Responses
{
    public class MasterResponseDetail
    {
        public string GroupName { get; set; }
        public string UserName { get; set; }
        public string SubjectName { get; set; }
        public int StudentsCount { get; set; }
        public int Session { get; set; }
        public int Course { get; set; }
        public int QuantityStudentAdvance { get; set; }
        public List<string> StudentNeededReInforce { get; set; }
        public List<string> StudentStillInSession { get; set; }
        public List<string> StudentNeverStartSession { get; set; }
        public List<ActivitiesAndState> Activities { get; set; }
    }

    public class ActivitiesAndState
    {
        public string Activity { get; set; }
        public float AdvancePercentage { get; set; }
        public float ReViewPercentage { get; set; }
        public float ReInforcePercentage { get; set; }
    }

    public class DetailListStudentAndCountPasses
    {
        public List<string> StudentNeededReInforce { get; set; }
        public List<string> StudentStillInSession { get; set; }
        public List<string> StudentNeverStartSession { get; set; }
        public int Quantity { get; set; }
    }
}
