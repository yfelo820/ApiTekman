using System;

namespace Api.DTO.Backoffice.Multimedia
{
    public class MultimediaListItem
    {
        public Guid Id { get; set; }
        public CourseDTO Course { get; set; }
        public string Title{ get; set; }

        public static MultimediaListItem ToListItem(Entities.Content.Multimedia multimedia)
        {
            return new MultimediaListItem
            {
                Id = multimedia.Id,
                Course = new CourseDTO {Id = multimedia.Course.Id, Number = multimedia.Course.Number},
                Title = multimedia.Title
            };
        }
    }
}