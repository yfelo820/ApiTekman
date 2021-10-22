using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Backoffice.Subject
{
    public class SubjectRequest
    {
        [Required] public string Name { get; set; }
        [Required] public int SessionCount { get; set; }
        [Required] public int DifficultyCount { get; set; }

        public static Entities.Content.Subject ToEntity(SubjectRequest subject)
        {
            return new Entities.Content.Subject
            {
                Name = subject.Name,
                SessionCount = subject.SessionCount,
                DifficultyCount = subject.DifficultyCount
            };
        }
    }
}