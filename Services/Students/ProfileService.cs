using System.Threading.Tasks;
using Api.Databases.Identity;
using Api.DTO.Students;
using Api.Identity.Models;
using Api.Interfaces.Students;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.Students
{
    public class ProfileService : IProfileService
    {
        private readonly ApiIdentityDbContext _identityDbContext;
        private readonly IGroupsService _groupService;

        public ProfileService(
            ApiIdentityDbContext identityDbContext,
            IGroupsService groupService
        )
        {
            _identityDbContext = identityDbContext;
            _groupService = groupService;
        }

        public async Task<StudentProfileDTO> GetProfileByEmail(string email)
        {
            var profile = await GetUserByEmail(email);
            var group = await _groupService.GetGroupByUsername(profile.User.UserName);

            return new StudentProfileDTO
            {
                Name = profile.Name,
                Email = profile.User.Email,
                Course = group.Course,
                SchoolCity = profile.SchoolCity,
                SchoolName = profile.SchoolName,
                ProfileType = profile.ProfileType
            };
        }

        public async Task UpdateProfile(string userName, UpdateStudentProfileDTO studentProfile)
        {
            var profile = await GetUserByEmail(userName);
            profile.Name = studentProfile.Name;
            profile.SchoolCity = studentProfile.SchoolCity;
            profile.SchoolName = studentProfile.SchoolName;
            profile.ProfileType = studentProfile.ProfileType;

            _identityDbContext.UniversalUserProperties.Update(profile);

            await _identityDbContext.SaveChangesAsync();
        }

        private async Task<UniversalUserProperties> GetUserByEmail(string email)
        {
            return await _identityDbContext.UniversalUserProperties.Include(u => u.User).FirstOrDefaultAsync(up => up.User.Email == email);
        }
    }
}
