using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class UserMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable(nameof(User));

            builder.Entity<User>().HasKey(x => x.Id);

            builder.Entity<User>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<User>().Property(x => x.DisplayName).IsRequired().HasMaxLength(256);

            builder.Entity<User>().Property(x => x.Email).IsRequired().HasMaxLength(256);

            builder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            builder.Entity<User>().Property(x => x.Password).IsRequired().HasMaxLength(256);
        }
    }
}