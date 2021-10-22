using System;

namespace Api.APIsMailing.Responses
{
    public class MasterResponseGlobal
    {
        public string SchoolId { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public int LastSessionStarted { get; set; }
        public int StudentsCount { get; set; }
        public string LastSessionCompleteDate { get; set; }
        public string PenultimateSessionCompleteDate { get; set; }
    }

    public class DateAndSession
    {
        public string Date { get; set; }
        public int Session { get; set; }
    }

    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string SchoolId { get; set; }
        public int Course { get; set; }
    }
}
