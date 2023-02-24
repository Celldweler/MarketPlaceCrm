using MarketPlaceCrm.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlaceCrm.Data.EntityConfigurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Replies)
                .WithOne(y => y.Parent);

            builder.HasOne(x => x.From)
                .WithMany(y => y.MyComments);
            
            builder.HasOne(x => x.Product)
                .WithMany(y => y.Reviews);
        }
    }
}