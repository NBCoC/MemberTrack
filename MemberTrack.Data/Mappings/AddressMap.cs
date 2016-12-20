using MemberTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MemberTrack.Data.Mappings
{
    public class AddressMap
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Entity<Address>().ToTable(nameof(Address));

            builder.Entity<Address>().HasKey(x => x.PersonId);

            builder.Entity<Address>().HasOne(x => x.Person).WithOne(x => x.Address).HasForeignKey<Address>(
                x => x.PersonId);

            builder.Entity<Address>().Property(x => x.Street).IsRequired().HasMaxLength(150);

            builder.Entity<Address>().Property(x => x.City).IsRequired().HasMaxLength(150);
        }
    }
}