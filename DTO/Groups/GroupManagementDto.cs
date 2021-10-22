using System;
using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Groups
{
    public class GroupManagementDto
    {
        [Required]
        public Guid TkGroupId { get; set; }
        [Required]
        public int Course { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SubjectKey { get; set; }
        [Required]
        public string LanguageKey { get; set; }
        [Required]
        public string SchoolId { get; set; }
    }
}