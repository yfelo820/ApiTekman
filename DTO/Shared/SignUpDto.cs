using Api.Validators;
using System.ComponentModel.DataAnnotations;

namespace Api.DTO.Shared
{
    public class SignUpDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }

        [Required]
        [Range(1, 6)]
        public int Course { get; set; }

        [Required]
        public string SchoolName { get; set; }

        [Required]
        public string SchoolCity { get; set; }

        [Required]
        public string ProfileType { get; set; }

        [Required]
        [IsTrue]
        public bool AcceptConditions { get; set; }

        [Required]
        public bool AcceptNewsletters { get; set; }

        [Required]
        public string RecaptchaToken { get; set; }
    }
}