using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Api.Attributes;
using Api.Entities.Content;

namespace Api.DTO.Backoffice.Multimedia
{
    public class MultimediaRequest
    {
        [Required, NotEmpty] 
        public Guid CourseId { get; set; }
        [Required, NotEmpty] 
        public Guid SubjectId { get; set; }
        [Required, NotEmpty] 
        public Guid LanguageId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string MediaUrl { get; set; }
        [RequiredEnum]
        public MediaTypeRequest? Type { get; set; }

        public static Entities.Content.Multimedia ToEntity(MultimediaRequest multimedia)
        {
            return new Entities.Content.Multimedia
            {
                CourseId = multimedia.CourseId,
                SubjectId = multimedia.SubjectId,
                LanguageId = multimedia.LanguageId,
                Title = multimedia.Title,
                Type = (MediaType) multimedia.Type.Value,
                FileName = multimedia.MediaUrl.Split("/").Last()
            };
        }
    }

    public enum MediaTypeRequest
    {
        Image,
        Video,
        Audio
    }
}