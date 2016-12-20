using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class ChildrenInfoMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<ChildrenInfo>().ToTable(nameof(ChildrenInfo));

            builder.Entity<ChildrenInfo>().HasKey(x => x.PersonId);

            builder.Entity<ChildrenInfo>().HasOne(x => x.Person).WithMany(x => x.ChildrenInfos).HasForeignKey(
                x => x.PersonId);
        }
    }
}