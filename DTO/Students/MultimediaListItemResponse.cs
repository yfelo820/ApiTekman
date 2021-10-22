using Api.Entities.Content;

namespace Api.DTO.Students
{
    public class MultimediaListItemResponse
    {
        public string Title { get; set; }
        public string MediaUrl { get; set; }

        public static MultimediaListItemResponse Map(Multimedia multimedia, string mediaUrlPath, string fileName)
        {
            return new MultimediaListItemResponse
            {
                Title = multimedia.Title,
                MediaUrl = $"{mediaUrlPath}/{multimedia.Id}/{fileName}"
            };
        }
    }
}
