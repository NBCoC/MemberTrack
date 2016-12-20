using MemberTrack.Data.Entities;
using MemberTrack.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentData> DocumentDatas { get; set; }

        public DbSet<DocumentTag> DocumentTags { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Visit> Visits { get; set; }

        public DbSet<VisitCheckListItem> VisitCheckListItems { get; set; }

        public DbSet<VisitCheckList> VisitCheckLists { get; set; }

        public DbSet<ChildrenInfo> ChildrenInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            UserMap.Configure(builder);
            AddressMap.Configure(builder);
            DocumentMap.Configure(builder);
            DocumentTagMap.Configure(builder);
            DocumentDataMap.Configure(builder);
            PersonMap.Configure(builder);
            VisitMap.Configure(builder);
            VisitCheckListItemMap.Configure(builder);
            VisitCheckListMap.Configure(builder);
            ChildrenInfoMap.Configure(builder);
        }
    }
}