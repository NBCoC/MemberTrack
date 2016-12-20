using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class DocumentMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Document>().ToTable(nameof(Document));

            builder.Entity<Document>().HasKey(x => x.Id);

            builder.Entity<Document>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<Document>().HasMany(x => x.DocumentTags).WithOne(x => x.Document).HasForeignKey(
                x => x.DocumentId);

            builder.Entity<Document>().HasOne(x => x.DocumentData).WithOne(x => x.Document).HasForeignKey<Document>(
                x => x.Id);

            builder.Entity<Document>().Property(x => x.Description).HasMaxLength(350);

            builder.Entity<Document>().Property(x => x.Name).HasMaxLength(256).IsRequired();

            builder.Entity<Document>().Property(x => x.Extension).HasMaxLength(5).IsRequired();
        }
    }
}