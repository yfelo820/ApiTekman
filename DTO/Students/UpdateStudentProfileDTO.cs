using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DTO.Students
{
    public class UpdateStudentProfileDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, 6)]
        public int Course { get; set; }
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string SchoolCity { get; set; }
        [Required]
        public string ProfileType { get; set; }
    }
}
