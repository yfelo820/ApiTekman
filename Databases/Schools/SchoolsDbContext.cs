using Api.Entities.Schools;
using Api.Entities.Schools.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Api.Databases.Schools
{
    public class SchoolsDbContext : DbContext
    {
        public SchoolsDbContext(DbContextOptions<SchoolsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<TeacherGroup> TeacherGroup { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<StudentGroup> StudentGroup { get; set; }
        public DbSet<StudentProgress> StudentProgress { get; set; }
        public DbSet<StudentAnswer> StudentAnswer { get; set; }
        public DbSet<StudentProblemsAnswer> StudentProblemsAnswer { get; set; }
        public DbSet<SsoIdentity> SsoIdentity { get; set; }
        public DbSet<ParentFeedbackQuestionSet> FeedbackQuestionSet { get; set; }
        public DbSet<ParentFeedbackAnswerSet> FeedbackAnswerSet { get; set; }
        public DbSet<PendingParentFeedback> PendingFeedback { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherGroupConfiguration());
            modelBuilder.ApplyConfiguration(new StudentAnswerConfiguration());
            modelBuilder.ApplyConfiguration(new StudentProblemsAnswerConfiguration());
            modelBuilder.ApplyConfiguration(new StudentGroupConfiguration());
            modelBuilder.ApplyConfiguration(new StudentProgressConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackAnswerSetConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackQuestionSetConfiguration());
            modelBuilder.ApplyConfiguration(new PendingFeedbackConfiguration());
        }
    }
}