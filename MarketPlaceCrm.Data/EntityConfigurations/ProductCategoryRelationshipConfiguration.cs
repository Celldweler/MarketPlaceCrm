using MarketPlaceCrm.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MarketPlaceCrm.Data.EntityConfigurations
{
    public class ProductCategoryRelationshipConfiguration : IEntityTypeConfiguration<ProductCategoryRelationship>
    {
        public void Configure(EntityTypeBuilder<ProductCategoryRelationship> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.CategoryId })
                .IsClustered();

            builder.HasOne(x => x.Product)
                .WithMany(y => y.Categories)
                .HasForeignKey(fk => fk.ProductId);

            builder.HasOne(x => x.Category)
                .WithMany(y => y.Products)
                .HasForeignKey(fk => fk.CategoryId);
        }
    }
}