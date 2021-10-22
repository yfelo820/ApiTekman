using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Entities.Schools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Entities.Schools.Configurations
{
    public class FeedbackAnswerSetConfiguration : IEntityTypeConfiguration<ParentFeedbackAnswerSet>
    {
        public void Configure(EntityTypeBuilder<ParentFeedbackAnswerSet> builder)
        {
            builder.Property(t => t.AnswerValue1).IsRequired();
            builder.Property(t => t.AnswerValue2).IsRequired();
            builder.Property(t => t.AnswerValue3).IsRequired();
            builder.Property(t => t.AnswerValue4).IsRequired();
            builder.Property(t => t.AnswerValue5).IsRequired();

            builder.Property(t => t.QuestionText1).IsRequired();
            builder.Property(t => t.QuestionText2).IsRequired();
            builder.Property(t => t.QuestionText3).IsRequired();
            builder.Property(t => t.QuestionText4).IsRequired();
            builder.Property(t => t.QuestionText5).IsRequired();

        }
    }
}
