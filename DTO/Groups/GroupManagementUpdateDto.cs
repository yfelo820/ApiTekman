using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Groups
{
    public class GroupManagementUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}