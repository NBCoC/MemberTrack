using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class TopicCategoryMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<TopicCategory>().ToTable(nameof(Quiz) + nameof(TopicCategory));

            builder.Entity<TopicCategory>().HasKey(x => x.Id);
            builder.Entity<TopicCategory>().HasAlternateKey(x => x.Name);

            builder.Entity<TopicCategory>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<TopicCategory>().Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.Entity<TopicCategory>().Property(x => x.Description).HasMaxLength(300);
        }
    }
}
