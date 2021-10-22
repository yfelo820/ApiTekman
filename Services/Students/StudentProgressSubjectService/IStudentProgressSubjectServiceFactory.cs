namespace Api.Services.Students.StudentProgressSubjectService
{
    public interface IStudentProgressSubjectServiceFactory
    {
        IStudentProgressSubjectService Create(string subject);
    }
}