using System;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities.Moderation;

namespace MarketPlaceCrm.Data.VersionMigrations
{
    public class VersionMigrationContext
    {
        private readonly AppDbContext _ctx;

        public VersionMigrationContext(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        private IEntityMigrationContext EntityMigrationContext { get; set; }
        private ModerationItem ModerationItem { get; set; }

        public VersionMigrationContext Setup(ModerationItem moderationItem)
        {
            ModerationItem = moderationItem ?? throw new ArgumentException(nameof(ModerationItem));
            EntityMigrationContext = ModerationItem.Type switch
            {
                ModerationTypes.NewAddedProduct => new ProductMigrationsContext(_ctx),
                ModerationTypes.UserComment => new ProductMigrationsContext(_ctx),
            };
            
            return this;
        }
    }
}