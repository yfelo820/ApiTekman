using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class StudentAnswerConfiguration : IEntityTypeConfiguration<StudentAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentAnswer> builder)
        {
            builder.Property(t => t.LanguageKey)
                .HasColumnType("VARCHAR(8)")
                .HasMaxLength(8);

            builder.Property(t => t.SubjectKey)
                .HasColumnType("VARCHAR(20)")
                .HasMaxLength(20);

            builder.Property(t => t.UserName)
                .HasColumnType("VARCHAR(450)")
                .HasMaxLength(450);

            builder
                .HasIndex(s => new { s.UserName, s.SubjectKey, s.LanguageKey })
                .HasName("Index_StudentAnswer_UserNameSubjectLang");
        }
    }
}
