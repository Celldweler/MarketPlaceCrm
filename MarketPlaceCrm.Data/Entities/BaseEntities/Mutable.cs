using System;

namespace MarketPlaceCrm.Data.Entities.BaseEntities
{
    public abstract class Mutable
    {
        public int Id { get; set; }
        
        public DateTime Updated { get; set; } = DateTime.Now;

        // updated by
        public ApplicationUser User { get; set; }
        public int UserId { get; set; }
    }
}