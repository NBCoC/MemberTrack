using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class VisitCheckListItemMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<VisitCheckListItem>().ToTable(nameof(VisitCheckListItem));

            builder.Entity<VisitCheckListItem>().HasKey(x => x.Id);

            builder.Entity<VisitCheckListItem>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<VisitCheckListItem>().Property(x => x.Description).IsRequired().HasMaxLength(300);

            builder.Entity<VisitCheckListItem>().HasIndex(x => x.Description).IsUnique();
        }
    }
}