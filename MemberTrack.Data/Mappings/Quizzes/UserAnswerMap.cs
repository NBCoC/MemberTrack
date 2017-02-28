using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class UserAnswerMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<UserAnswer>().ToTable(nameof(Quiz) + nameof(UserAnswer));

            builder.Entity<UserAnswer>().HasKey(x => x.Id);

            builder.Entity<UserAnswer>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<UserAnswer>().HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired();

            builder.Entity<UserAnswer>().HasOne(x => x.Answer).WithMany().HasForeignKey(x => x.AnswerId).IsRequired();

        }
    }
}
