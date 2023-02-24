using MarketPlaceCrm.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlaceCrm.Data.EntityConfigurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            // builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Orders)
                .WithOne(x => x.Customer)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}