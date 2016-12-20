using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class VisitMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Visit>().ToTable(nameof(Visit));

            builder.Entity<Visit>().HasKey(x => x.VisitorId);

            builder.Entity<Visit>().HasOne(x => x.Visitor).WithMany(x => x.Visits).HasForeignKey(x => x.VisitorId);

            builder.Entity<Visit>().Property(x => x.Note).HasMaxLength(300);

            builder.Entity<Visit>().HasIndex(x => x.Date).IsUnique();
        }
    }
}