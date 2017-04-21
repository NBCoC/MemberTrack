using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class PersonMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Person>().ToTable(nameof(Person));

            builder.Entity<Person>().HasKey(x => x.Id);

            builder.Entity<Person>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<Person>().Property(x => x.Email).HasMaxLength(256);

            builder.Entity<Person>().Property(x => x.ContactNumber).HasMaxLength(15);

            builder.Entity<Person>().Property(x => x.Description).HasMaxLength(500);

            builder.Entity<Person>().Property(x => x.FullName).IsRequired().HasMaxLength(150);

            builder.Entity<Person>().HasMany(x => x.CheckLists).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);

            builder.Entity<Person>().Property(x => x.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}