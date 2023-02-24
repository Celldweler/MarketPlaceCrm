using System;
using System.Collections.Generic;
using MarketPlaceCrm.Data.Entities.BaseEntities;

namespace MarketPlaceCrm.Data.Entities
{
    public class Category : VersionEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public bool Deleted { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int ParentID { get; set; }
        public Category? Parent{ get; set; }

        public List<Category> SubCategories { get; set; }

        public List<ProductCategoryRelationship> Products { get; set; }
    }

    public class ProductCategoryRelationship
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}