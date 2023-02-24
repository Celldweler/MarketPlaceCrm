using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities.BaseEntities;

namespace MarketPlaceCrm.Data.VersionMigrations
{
    public class ProductMigrationsContext : IEntityMigrationContext
    {
        private readonly AppDbContext _ctx;

        public ProductMigrationsContext(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public IQueryable<VersionEntity> GetSource()
        {
            return _ctx.Products;
        }

        public void Migrate(int current, int target)
        {
            throw new System.NotImplementedException();
        }
    }
}