using System;
using MarketPlaceCrm.Data.Entities.BaseEntities;
using MarketPlaceCrm.Data.Enums;

namespace MarketPlaceCrm.Data.Entities.Moderation
{
    public class Review 
    {
        public int Id { get; set; }
        
        public DateTime Updated { get; set; } = DateTime.Now;
        public string Comment { get; set; }
        public ReviewStatus ReviewStatus { get; set; }

        // updated by
        public ApplicationUser User { get; set; }
        public int UserId { get; set; }

        public ModerationItem ModerationItem { get; set; }
        public int ModerationItemID { get; set; }
    }
}