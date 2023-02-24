using System;
using System.Collections.Generic;

namespace MarketPlaceCrm.Data.Entities.Moderation
{
    public class ModerationItem
    {
        public int Id { get; set; }

        public int CurrentId { get; set; }
        public int TargetId { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;
        public string Type { get; set; }
        public bool IsRejected { get; set; } = false;

        public ApplicationUser User { get; set; }
        public int UserId { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}