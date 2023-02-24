using System;
using MarketPlaceCrm.Data.Entities.BaseEntities;

namespace MarketPlaceCrm.Data.Entities
{
    public class Stock : VersionEntity
        // BaseEntity<int>
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Deleted { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}