using System;
using MediatR;

namespace Api.Commands.AddGroup
{
    public class AddGroupCommand : IRequest
    {
        public Guid TkGroupId { get; }
        public int Course { get; }
        public string Name { get; }
        public string SubjectKey { get; }
        public string LanguageKey { get; }
        public string SchoolId { get; }

        public AddGroupCommand(Guid tkGroupId, int course, string name, string subjectKey, string languageKey, string schoolId)
        {
            TkGroupId = tkGroupId;
            Course = course;
            Name = name;
            SubjectKey = subjectKey;
            LanguageKey = languageKey;
            SchoolId = schoolId;
        }
    }
}