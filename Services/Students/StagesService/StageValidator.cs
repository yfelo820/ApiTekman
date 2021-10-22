using System.Threading.Tasks;
using Api.Exceptions;
using Api.Interfaces.Students;

namespace Api.Services.Students.StagesService
{
    public class StageValidator : IStageValidator
    {
        private readonly IStudentProgressService _studentProgressService;

        public StageValidator(IStudentProgressService studentProgressService)
        {
            _studentProgressService = studentProgressService;
        }

        public async Task Validate(string userName, string subject, string language, int course, int session)
        {
            var studentProgress = await _studentProgressService.Get(userName, subject, language);
            var currentCourse = studentProgress.Course;
            var currentSession = studentProgress.Session;
            if (course > currentCourse || (course == currentCourse && session > currentSession))
            {
                throw new BadRequestException("INVALID_SESSION_ACCESS",
                    $@"It's not possible to get the stage activity from course {course} and session {session}. 
                        Student progress: userName: {userName}, Subject: {subject}, Course: {currentCourse} and Session: {currentSession}.");
            }
        }
    }
}
