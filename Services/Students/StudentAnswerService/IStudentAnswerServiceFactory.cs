namespace Api.Services.Students.StudentAnswerService
{
    public interface IStudentAnswerServiceFactory
    {
        IStudentAnswerService Create(string subject);
    }
}
