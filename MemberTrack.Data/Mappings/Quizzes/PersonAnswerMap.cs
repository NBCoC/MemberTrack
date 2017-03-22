using MemberTrack.Data.Entities.Quizzes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MemberTrack.Data.Mappings.Quizzes
{
    public class PersonAnswerMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<PersonAnswer>().ToTable(nameof(Quiz) + nameof(PersonAnswer));

            builder.Entity<PersonAnswer>().HasKey(x => x.Id);

            builder.Entity<PersonAnswer>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<PersonAnswer>().HasOne(x => x.Person).WithMany()
                .HasForeignKey(x => x.PersonId).IsRequired().OnDelete(DeleteBehavior.Cascade); ;

            builder.Entity<PersonAnswer>().HasOne(x => x.Answer).WithMany()
                .HasForeignKey(x => x.AnswerId).IsRequired().OnDelete(DeleteBehavior.Cascade); ;

        }
    }
}
