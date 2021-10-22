using Microsoft.AspNetCore.Identity;

namespace Api.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UniversalUserProperties UniversalUserProperties { get; set; }
    }
}