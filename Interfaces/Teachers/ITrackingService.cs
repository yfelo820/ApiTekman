using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Teachers;
using Api.Entities.Schools;
using Api.Services.Teachers;

namespace Api.Interfaces.Teachers
{
    public interface ITrackingService
    {
        Task<GroupTrackingResponse> GetSingle(Guid id);
        Task<StudentTrackingExtendedDTO> GetSingleStudentDetail(string userName);
        Task<StudentProgress> UpdateStudentProgress(StudentProgress progress);
        Task<GroupSessionProgress> GetGroupSessionProgress(Guid groupID, int session);
        Task<GroupBlockProgress> GetGroupBlockProgress(Guid groupID);
        Task<GroupBlockProgress> GetStudentSuperBlockProgress(Guid groupID, string userName);
        Task<StudentsListSubjectsAverage> GetStudentsListSubjectsAverage(Guid groupID);
        Task<StudentProgress> SetStudentToAnotherSession(Guid groupID, string userName, int session, int course);
        Task<StudentProgress> ResetDiagnosisTestState(Guid groupID, string userName);
        Task<CoursesPerStudent> GetCoursesPerStudent(Guid groupID, string userName);
        Task<StagesPerSession> GetStudentSessionProgress(Guid groupID, string userName, int course, int session);
        Task<List<ActivitiesOfStage>> GetActivitiesOfStage(Guid groupID, string userName, int course, int session, int stage);
    }
}
