using Api.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Databases.Content.Configurations
{
    public class MultimediaConfiguration : IEntityTypeConfiguration<Multimedia>
    {
        
        public void Configure(EntityTypeBuilder<Multimedia> builder)
        {
            builder.Property(m => m.Title)
                .HasColumnType("VARCHAR(150)")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(m => m.FileName)
                .HasColumnType("VARCHAR(150)")
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}