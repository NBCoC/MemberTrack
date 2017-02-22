using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class PersonCheckListMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<PersonCheckList>().ToTable(nameof(PersonCheckList));

            builder.Entity<PersonCheckList>().HasKey(x => new {x.PersonId, x.PersonCheckListItemId});

            builder.Entity<PersonCheckList>().HasOne(x => x.Person).WithMany(x => x.CheckLists).HasForeignKey(
                x => x.PersonId);

            builder.Entity<PersonCheckList>().HasOne(x => x.PersonCheckListItem).WithMany(x => x.CheckLists)
                .HasForeignKey(x => x.PersonCheckListItemId);

            builder.Entity<PersonCheckList>().Property(x => x.Note).HasMaxLength(500);
        }
    }
}