using System;
using System.Collections.Generic;
using Api.Entities.Content;

namespace Api.DTO.Students
{
    public class SubjectResponse
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public IEnumerable<SubjectCourseDTO> Courses { get; set; }
        public int StagesInSession { get; set; }

        public static SubjectResponse Map(Subject subject)
        {
            return new SubjectResponse {Key = subject.Key, Name = subject.Name};
        }
    }
}