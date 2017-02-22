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

            builder.Entity<Person>().HasOne(x => x.Address).WithOne(x => x.Person).HasPrincipalKey<Person>(x => x.Id);

            builder.Entity<Person>().Property(x => x.Email).HasMaxLength(256);

            builder.Entity<Person>().HasIndex(x => x.Email).IsUnique();

            builder.Entity<Person>().Property(x => x.ContactNumber).HasMaxLength(15);

            builder.Entity<Person>().Property(x => x.FirstName).IsRequired().HasMaxLength(75);

            builder.Entity<Person>().Property(x => x.MiddleName).HasMaxLength(75);

            builder.Entity<Person>().Property(x => x.LastName).IsRequired().HasMaxLength(75);

            builder.Entity<Person>().HasMany(x => x.CheckLists).WithOne(x => x.Person).HasForeignKey(x => x.PersonId);
        }
    }
}