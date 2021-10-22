using Api.Entities.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class TeacherGroupConfiguration : IEntityTypeConfiguration<TeacherGroup>
    {
        public void Configure(EntityTypeBuilder<TeacherGroup> builder)
        {
            builder.Property(t => t.TeacherId)
                .IsRequired();

            builder.Property(t => t.GroupId)
                .IsRequired();

            // Index - Teacher
            builder
                .HasIndex(g => new { g.TeacherId })
                .HasName("IX_TeacherId");

            // Index - GroupId
            builder
                .HasIndex(g => new { g.GroupId })
                .HasName("IX_GroupId");
        }
    }
}
