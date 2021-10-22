using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class FeedbackQuestionSetConfiguration : IEntityTypeConfiguration<ParentFeedbackQuestionSet>
    {
        public void Configure(EntityTypeBuilder<ParentFeedbackQuestionSet> builder)
        {
            builder.Property(t => t.QuestionLabel1)
                .IsRequired();
            builder.Property(t => t.QuestionLabel2)
                .IsRequired();
            builder.Property(t => t.QuestionLabel3)
                .IsRequired();
            builder.Property(t => t.QuestionLabel4)
                .IsRequired();
            builder.Property(t => t.QuestionLabel5)
                .IsRequired();
            builder.Property(t => t.QuestionSetType)
                .IsRequired();

        }
    }
}
