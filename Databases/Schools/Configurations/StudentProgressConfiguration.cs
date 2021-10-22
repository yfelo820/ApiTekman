using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class StudentProgressConfiguration : IEntityTypeConfiguration<StudentProgress>
    {
        public void Configure(EntityTypeBuilder<StudentProgress> builder)
        {
            builder
                .HasIndex(s => new { s.UserName, s.SubjectKey, s.LanguageKey })
                .HasName("Index_StudentProgress_UniqueSubject")
                .IsUnique();
        }
    }
}
