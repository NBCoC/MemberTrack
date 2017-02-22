namespace MemberTrack.Data
{
    using Entities;
    using Mappings;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<PersonCheckListItem> PersonCheckListItems { get; set; }

        public DbSet<PersonCheckList> PersonCheckLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            UserMap.Configure(builder);
            AddressMap.Configure(builder);
            PersonMap.Configure(builder);
            PersonCheckListItemMap.Configure(builder);
            PersonCheckListMap.Configure(builder);
        }
    }
}