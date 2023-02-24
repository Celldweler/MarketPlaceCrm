using System.Linq;
using MarketPlaceCrm.Data.Entities.BaseEntities;

namespace MarketPlaceCrm.Data.VersionMigrations
{
    public interface IEntityMigrationContext
    {
        IQueryable<VersionEntity> GetSource();
        void Migrate(int current, int target);
    }
}