using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class DocumentTagMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<DocumentTag>().ToTable(nameof(DocumentTag));

            builder.Entity<DocumentTag>().HasKey(x => x.Id);

            builder.Entity<DocumentTag>().Property(x => x.Id).UseSqlServerIdentityColumn();

            builder.Entity<DocumentTag>().HasOne(x => x.Document).WithMany(x => x.DocumentTags).HasForeignKey(
                x => x.DocumentId);

            builder.Entity<DocumentTag>().Property(x => x.Value).HasMaxLength(75).IsRequired();
        }
    }
}