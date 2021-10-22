using Api.Entities.Content;

namespace Api.DTO.Backoffice.Multimedia
{
    public class MultimediaResponse
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string SubjectId { get; set; }
        public int SubjectSessionCount { get; set; }
        public int SubjectDifficultyCount { get; set; }
        public string LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string Title { get; set; }
        public MediaTypeRequest Type { get; set; }
        public string MediaUrl { get; set; }

        public static MultimediaResponse Map(Entities.Content.Multimedia multimedia, string mediaUrlPath,
            string fileName)
        {
            return new MultimediaResponse
            {
                Id = multimedia.Id.ToString(),
                Title = multimedia.Title,
                CourseId = multimedia.CourseId.ToString(),
                SubjectId = multimedia.Subject.Id.ToString(),
                SubjectSessionCount = multimedia.Subject.SessionCount,
                SubjectDifficultyCount = multimedia.Subject.DifficultyCount,
                LanguageId = multimedia.Language.Id.ToString(),
                LanguageName = multimedia.Language.Name,
                Type = (MediaTypeRequest) multimedia.Type,
                MediaUrl = $"{mediaUrlPath}/{multimedia.Id}/{fileName}"
            };
        }
    }
}