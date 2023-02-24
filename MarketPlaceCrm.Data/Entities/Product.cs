using System;
using System.Collections.Generic;
using MarketPlaceCrm.Data.Entities.BaseEntities;
using MarketPlaceCrm.Data.Enums;

namespace MarketPlaceCrm.Data.Entities
{
    public class Product : VersionEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool Deleted { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }

        public string Image { get; set; }
        public bool IsPublished { get; set; }
        
        public VersionState State { get; set; }

        // public List<Image> Images { get; set; }
        //
        public List<Stock> Stocks { get; set; } = new List<Stock>();
        public List<ProductCategoryRelationship> Categories { get; set; }
        public List<Comment> Reviews { get; set; } = new List<Comment>();
    }
}