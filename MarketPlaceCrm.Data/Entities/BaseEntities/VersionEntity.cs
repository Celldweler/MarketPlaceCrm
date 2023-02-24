using System;
using MarketPlaceCrm.Data.Enums;

namespace MarketPlaceCrm.Data.Entities.BaseEntities
{
    public abstract class VersionEntity 
    {
        public int Version { get; set; } = 1;
        public DateTime Updated { get; set; } = DateTime.Now;
        public VersionState VersionState { get; set; } = VersionState.Live;

        // updated by
        public ApplicationUser User { get; set; }
        public int UserId { get; set; }
    }
}