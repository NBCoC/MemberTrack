using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class TopicMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Topic>().ToTable(nameof(Quiz) + nameof(Topic));

            builder.Entity<Topic>().HasKey(x => x.Id);

            builder.Entity<Topic>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<Topic>().Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Entity<Topic>().Property(x => x.Description).HasMaxLength(300);

            builder.Entity<Topic>().HasOne(x => x.TopicCategory).WithMany(x => x.Topics).HasForeignKey(x => x.TopicCategoryId).IsRequired();

        }
    }
}
