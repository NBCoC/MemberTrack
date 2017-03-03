using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class SupportingScriptureMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<SupportingScripture>().ToTable(nameof(Quiz) + nameof(SupportingScripture));

            builder.Entity<SupportingScripture>().HasKey(x => x.Id);

            builder.Entity<SupportingScripture>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<SupportingScripture>().Property(x => x.ScriptureReference).IsRequired().HasMaxLength(50);

            builder.Entity<SupportingScripture>().Property(x => x.Description).HasMaxLength(300);

            builder.Entity<SupportingScripture>().HasOne(x => x.Topic).WithMany(x => x.SupportingScriptures)
                                .HasForeignKey(x => x.TopicId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        }

    }
}
