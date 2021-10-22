using System;

namespace Api.DTO.Shared
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string Photo { get; set; }
        public string Role { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public string SchoolId { get; set; }
        public string SchoolName { get; set; }

        public Guid TokenId { get; set; }

        public string[] Languages { get; set; }
        
        public string GroupKey { get; set; }
    }
}
