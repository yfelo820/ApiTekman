using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(t => t.Key)
                .IsRequired();

            builder.Property(t => t.SubjectKey)
                .IsRequired();

            builder.Property(t => t.Name)
                .IsRequired();

            builder.Property(t => t.Course)
                .IsRequired();

            builder.Property(t => t.SchoolId)
                .IsRequired();

            builder.Property(t => t.LanguageKey)
                .IsRequired();

            // Index - Key
            builder
                .HasIndex(g => new { g.Key })
                .HasName("Index_Group_UniqueKey")
                .IsUnique();

            // Index - TKGroupId
            builder
                .HasIndex(g => new {g.TkGroupId})
                .HasName("IX_TkGroupId");
        }
    }
}
