using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class VisitCheckListMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<VisitCheckList>().ToTable(nameof(VisitCheckList));

            builder.Entity<VisitCheckList>().HasKey(x => new {x.VisitorId, x.VisitCheckListItemId});

            builder.Entity<VisitCheckList>().HasOne(x => x.Visit).WithMany(x => x.CheckList).HasForeignKey(
                x => x.VisitorId);

            builder.Entity<VisitCheckList>().HasOne(x => x.VisitCheckListItem).WithMany(x => x.CheckList)
                .HasForeignKey(x => x.VisitCheckListItemId);
        }
    }
}