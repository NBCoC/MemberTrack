using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class DocumentDataMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<DocumentData>().ToTable(nameof(DocumentData));

            builder.Entity<DocumentData>().HasKey(x => x.DocumentId);

            builder.Entity<DocumentData>().HasOne(x => x.Document).WithOne(x => x.DocumentData)
                .HasForeignKey<DocumentData>(x => x.DocumentId);

            builder.Entity<DocumentData>().Property(x => x.Data).IsRequired();
        }
    }
}