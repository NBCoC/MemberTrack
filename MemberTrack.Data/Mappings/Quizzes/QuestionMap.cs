using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class QuestionMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Question>().ToTable(nameof(Quiz) + nameof(Question));

            builder.Entity<Question>().HasKey(x => x.Id);

            builder.Entity<Question>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<Question>().Property(x => x.Description).IsRequired().HasMaxLength(300);

            builder.Entity<Question>().Property(x => x.RandomizeAnswers).IsRequired();

            builder.Entity<Question>().Property(x => x.AllowMultipleAnswers).IsRequired();

            builder.Entity<Question>().HasOne(x => x.Quiz).WithMany(x => x.Questions)
                .HasForeignKey(x => x.QuizId).IsRequired().OnDelete(DeleteBehavior.Cascade);

        }
    }
}
