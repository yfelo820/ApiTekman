using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTO.Students;
using Api.DTO.Teachers;
using Api.Entities.Content;
using Api.Entities.Schools;

namespace Api.Interfaces.Students
{
    public interface IParentFeedbackService
    {
        Task<ParentFeedbackQuestionDTO> GetQuestionSet(string userName);
        Task<List<ParentFeedbackAnswerDTO>> GetAnswers(string userName);
        Task<List<ParentFeedbackAverageValuationDTO>> GetAverageValuationsAndComments(List<string> studentsUsernames);
        Task Put(ParentFeedbackAnswerDTO parentFeedbackAnswer);
        Task NotifyFeedbackIfNecessary(StudentProgress currentProgress);
        Task UpsertPendingFeedback(PendingParentFeedbackDTO pendingFeedback);
        Task DeletePendingFeedback(string userName);
        Task<List<PendingParentFeedbackDTO>> GetPendingFeedback(List<string> userName);
    }
}
