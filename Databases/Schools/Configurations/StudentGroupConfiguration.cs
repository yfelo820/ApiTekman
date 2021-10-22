using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class StudentGroupConfiguration : IEntityTypeConfiguration<StudentGroup>
    {
        public void Configure(EntityTypeBuilder<StudentGroup> builder)
        {
            builder.Property(t => t.UserName)
                .IsRequired();

            builder.Property(t => t.GroupId)
                .IsRequired();

            builder
                .HasIndex(sg => new { sg.GroupId, sg.UserName })
                .HasName("Index_StudentGroup_UniqueStudentGroup")
                .IsUnique();

            builder
                .HasIndex(sg => new { sg.GroupId, sg.AccessNumber })
                .HasName("Index_StudentGroup_UniqueAccessNumber")
                .IsUnique();
        }
    }
}
