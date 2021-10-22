using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class PendingFeedbackConfiguration : IEntityTypeConfiguration<PendingParentFeedback>
    {
        public void Configure(EntityTypeBuilder<PendingParentFeedback> builder)
        {
            builder.Property(t => t.RequestTime)
                .IsRequired();
            builder.Property(t => t.UserName)
                .IsRequired();
        }
    }
}
