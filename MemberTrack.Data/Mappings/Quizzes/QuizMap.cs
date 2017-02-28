using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class QuizMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Quiz>().ToTable(nameof(Quiz));

            builder.Entity<Quiz>().HasKey(x => x.Id);

            builder.Entity<Quiz>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<Quiz>().Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Entity<Quiz>().Property(x => x.Description).HasMaxLength(300);

            builder.Entity<Quiz>().Property(x => x.Instructions);

            builder.Entity<Quiz>().Property(x => x.RandomizeQuestions).IsRequired();

        }
    }
}
