using Api.APIsMailing.Interfaces;
using Api.APIsMailing.Services;
using Api.Databases.Content;
using Api.Databases.Schools;
using Api.DTO.Teachers;
using Api.Entities.Content;
using Api.Factories;
using Api.Filters;
using Api.Interfaces.Backoffice;
using Api.Interfaces.Groups;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Api.Interfaces.Teachers;
using Api.Services.Backoffice;
using Api.Services.Shared;
using Api.Services.Students;
using Api.Services.Students.ParentFeedbackService;
using Api.Services.Students.StagesService;
using Api.Services.Students.StudentAnswerService;
using Api.Services.Students.StudentProgressSubjectService;
using Api.Services.Teachers;
using Api.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterRepositories(services);
            RegisterBackofficeServices(services);
            RegisterSharedServices(services);
            RegisterTeachersServices(services);
            RegisterStudentsServices(services);
            RegisterGroupsServices(services);
            RegisterTKReportsServices(services);
            RegisterSettings(services, configuration);
            RegisterParentServices(services);

            return services;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(ISchoolsRepository<>), typeof(SchoolsRepository<>));
            services.AddScoped(typeof(ISchoolsRepositoryUow<>), typeof(SchoolsRepositoryUow<>));
            services.AddScoped(typeof(IContentRepository<>), typeof(ContentRepository<>));
        }

        private static void RegisterBackofficeServices(IServiceCollection services)
        {         
            services.AddScoped<IApiService<ContentBlock>, ContentBlocksService>();
            services.AddScoped<IApiService<SuperContentBlock>, SuperContentBlockService>();
            services.AddScoped<IApiService<Activity>, Services.Backoffice.ActivitiesService>();
            services.AddScoped<IApiService<Transition>, TransitionsService>();
            services.AddScoped<IApiService<Exercise>, Services.Backoffice.ExercisesService>();
            services.AddScoped<IApiService<Template>, TemplatesService>();
            services.AddScoped<IApiService<Feedback>, Services.Backoffice.FeedbacksService>();
            services.AddScoped<IApiService<Achievement>, AchievementsService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<IApiService<ProblemResolution>, ProblemResolutionsService>();

            services.AddScoped<ILanguagesService, LanguagesService>();
            services.AddScoped<IMultimediaService, MultimediaService>();
            services.AddScoped<Services.Backoffice.ICoursesService, Services.Backoffice.CoursesService>();
        }

        private static void RegisterSharedServices(IServiceCollection services)
        {
            services.AddScoped<IMasterServiceDetail, MasterServiceDetail>();
            services.AddScoped<IMasterServiceGlobal, MasterServiceGlobal>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ISsoService, SsoService>();
            services.AddScoped<IUniversalStudentAuthService, UniversalStudentAuthService>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IRecaptchaService, RecaptchaService>();
            services.AddScoped<LanguageCheckerActionFilter>();
            services.AddScoped<IHttpContextService, HttpContextService>();
            services.AddScoped<ISubjectsService, SubjectsService>();
        }

        private static void RegisterTeachersServices(IServiceCollection services)
        {
            services.AddScoped<IApiService<GroupDTO>, Services.Teachers.GroupsService>();
            services.AddScoped<Interfaces.Teachers.ICoursesService, Services.Teachers.CoursesService>();
            services.AddScoped<IStudentsService, StudentsService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ITeacherGroupService, TeacherGroupService>();
            services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<ISessionsService, SessionsService>();
            services.AddScoped<IActivitiesService, Services.Teachers.ActivitiesService>();
            services.AddScoped<IStudentInfoExport, StudentInfoExport>();
            services.AddScoped<IStudentResultsExport, StudentResultsExport>();
            services.AddScoped<ICompleteSessionCalculatorFactory, CompleteSessionCalculatorFactory>();
        }

        private static void RegisterStudentsServices(IServiceCollection services)
        {
            services.AddScoped<IItineraryService, ItineraryService>();

            services.AddScoped<IEmatStageService, EmatStageService>();
            services.AddScoped<IEmatInfantilStageService, EmatInfantilStageService>();
            services.AddScoped<ILudiStageService, LudiStageService>();
            services.AddScoped<ISuperletrasStageService, SuperletrasStageService>();
            services.AddScoped<IStageServiceFactory, StageServiceFactory>();
            services.AddScoped<IStageValidator, StageValidator>();

            services.AddScoped<IEmatStudentAnswerService, EmatStudentAnswerService>();
            services.AddScoped<IEmatInfantilStudentAnswerService, EmatInfantilStudentAnswerService>();
            services.AddScoped<ILudiStudentAnswerService, LudiStudentAnswerService>();
            services.AddScoped<ISuperletrasStudentAnswerService, SuperletrasStudentAnswerService>();
            services.AddScoped<IStudentAnswerServiceFactory, StudentAnswerServiceFactory>();

            services.AddScoped<IExercisesService, Services.Students.ExercisesService>();
            services.AddScoped<IFeedbacksService, Services.Students.FeedbacksService>();
            services.AddScoped<IDiagnosisTestService, DiagnosisTestService>();
            services.AddScoped<IGroupsService, Services.Students.GroupsService>();
            services.AddScoped<IStudentProgressService, StudentProgressService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IStudentProblemsAnswerService, StudentProblemsAnswerService>();

            services.AddScoped<StudentProgressSubjectService>();
            services.AddScoped<EmatInfantilStudentProgressService>();
            services.AddScoped<BilingualStudentProgressService>();
            services.AddScoped<IStudentProgressSubjectServiceFactory, StudentProgressSubjectServiceFactory>();
        }

        private static void RegisterGroupsServices(IServiceCollection services)
        {
            services.AddScoped<IGroupsSharedService, GroupsSharedService>();
            services.AddScoped<IGroupSharedServiceUow, GroupsSharedServiceUow>();
            services.AddScoped<IGroupFactory, GroupFactory>();
        }

        private static void RegisterTKReportsServices(IServiceCollection services)
        {
            services.AddScoped<Interfaces.TkReports.IGroupsService, Services.TkReports.GroupsService>();
            services.AddScoped<Interfaces.TkReports.IItineraryService, Services.TkReports.ItineraryService>();
        }

        private static void RegisterSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GoogleAnalyticsSettings>(configuration.GetSection(SettingKey.GoogleAnalytics));
            services.Configure<RecaptchaSettings>(configuration.GetSection(SettingKey.Recaptcha));
            services.Configure<SupportedMediaTypes>(configuration.GetSection(SettingKey.SupportedMediaTypes));
            services.Configure<CultureLanguagesSettings>(configuration.GetSection(SettingKey.CultureLanguages));
            services.Configure<AzureStorageSettings>(configuration.GetSection(SettingKey.AzureStorage));
            services.Configure<MultimediaSettings>(configuration.GetSection(SettingKey.Multimedia));
        }

        private static void RegisterParentServices(IServiceCollection services)
        {
            services.AddScoped<IParentFeedbackService, ParentFeedbackService>();
            services.AddScoped<Api.Interfaces.Parents.IStudentsService, Api.Services.Parents.StudentsService>();
            services.AddScoped<Api.Interfaces.Parents.ITrackingService, Api.Services.Parents.TrackingService>();
        }
    }
}
