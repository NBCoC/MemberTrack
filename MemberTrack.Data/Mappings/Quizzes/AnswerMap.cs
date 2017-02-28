using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class AnswerMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Answer>().ToTable(nameof(Quiz) + nameof(Answer));

            builder.Entity<Answer>().HasKey(x => x.Id);

            builder.Entity<Answer>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<Answer>().Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Entity<Answer>().Property(x => x.Description).IsRequired().HasMaxLength(150);

            builder.Entity<Answer>().HasOne(x => x.Question).WithMany(x => x.Answers).HasForeignKey(x => x.QuestionId);

            builder.Entity<Answer>().HasOne(x => x.Topic).WithMany(x => x.Answers).HasForeignKey(x => x.TopicId).IsRequired();

            builder.Entity<Answer>().Property(x => x.TopicWeight).IsRequired();
        }

    }
}
