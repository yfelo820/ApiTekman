using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Students;
using Api.Filters;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Students
{
    [ApiController]
    [Route("students/[controller]")]
    [Authorize(Policy = Role.Student)]
    [ServiceFilter(typeof(LanguageCheckerActionFilter))]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IGroupsService _groupsService;
        private readonly IStudentProgressService _studentProgressService;
        private readonly IClaimsService _claimsService;

        public ProfileController(IClaimsService claimsService, 
            IProfileService profileService,
            IGroupsService groupsService,
            IStudentProgressService studentProgressService)
        {
            _profileService = profileService;
            _groupsService = groupsService;
            _studentProgressService = studentProgressService;
            _claimsService = claimsService;
        }

        [HttpGet]
        public async Task<StudentProfileDTO> Get()
        {
            var userName = _claimsService.GetUserName();
            return await _profileService.GetProfileByEmail(userName);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateStudentProfileDTO studentProfile)
        {
            var userName = _claimsService.GetUserName();
            await _profileService.UpdateProfile(userName, studentProfile);

            var group = await _groupsService.GetGroupByUsername(userName);
            if (group.Course != studentProfile.Course)
            {
                await _groupsService.UpdateStudentGroup(group.Id, studentProfile.Course, userName);
                await _studentProgressService.ResetStudentProgress(
                  userName,
                  studentProfile.Course,
                  UniversalStudent.SubjectKey,
                  UniversalStudent.LanguageKey
                );
            }

            return NoContent();
        }
    }
}
