using System;
using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Backoffice.Subject
{
    public class SubjectResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int SessionCount { get; set; }
        public int DifficultyCount { get; set; }

        public static SubjectResponse Map(Entities.Content.Subject subject)
        {
            return new SubjectResponse
            {
                Id = subject.Id,
                Name = subject.Name,
                SessionCount = subject.SessionCount,
                DifficultyCount = subject.DifficultyCount
            };
        }
    }
}