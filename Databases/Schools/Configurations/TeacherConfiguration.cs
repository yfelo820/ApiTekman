using System;
using System.Collections.Generic;
using Api.Entities.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(t => t.Name)
                .IsRequired();            

            builder.Property(t => t.Surnames)
                .IsRequired();

            builder.Property(t => t.Email)
                .IsRequired();

            builder.Property(t => t.SchoolId)
                .IsRequired();            
        }
    }
}
