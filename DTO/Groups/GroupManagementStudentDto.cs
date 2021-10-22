using System;
using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Groups
{
    public class GroupManagementStudentDto
    {
        [Required]
        public Guid TkStudentId { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}