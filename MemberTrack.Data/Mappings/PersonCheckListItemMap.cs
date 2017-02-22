using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class PersonCheckListItemMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<PersonCheckListItem>().ToTable(nameof(PersonCheckListItem));

            builder.Entity<PersonCheckListItem>().HasKey(x => x.Id);

            builder.Entity<PersonCheckListItem>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<PersonCheckListItem>().Property(x => x.Description).IsRequired().HasMaxLength(300);

            builder.Entity<PersonCheckListItem>().HasIndex(x => x.Description).IsUnique();
        }
    }
}