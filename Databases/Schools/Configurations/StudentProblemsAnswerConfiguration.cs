using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class StudentProblemsAnswerConfiguration : IEntityTypeConfiguration<StudentProblemsAnswer>
    {
        public void Configure(EntityTypeBuilder<StudentProblemsAnswer> builder)
        {
            builder.Property(t => t.SubjectKey)
                .HasColumnType("VARCHAR(20)")
                .HasMaxLength(20);

            builder.Property(t => t.UserName)
                .HasColumnType("VARCHAR(450)")
                .HasMaxLength(450);
        }
    }
}
