using System;

namespace MarketPlaceCrm.Data.Entities
{
    public abstract class BaseEntity<Tkey> : BaseEntityWithKey<Tkey>, IEntity  
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Deleted { get; set; }
    }
}