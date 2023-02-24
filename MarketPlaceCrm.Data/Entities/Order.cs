using System;
using System.Collections.Generic;
using MarketPlaceCrm.Data.Entities.BaseEntities;
using MarketPlaceCrm.Data.Enums;

namespace MarketPlaceCrm.Data.Entities
{
    public class Order : VersionEntity, IEntity 
    {
        public int Id { get; set; }
        public bool Deleted { get; set; }
        public DateTime Created { get; set; }= DateTime.Now;
        
        public OrderStatuses Status { get; set; }

        public string ShipperName { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public decimal Freight { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCountry { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipZipCode { get; set; }

        public Shipper? Shipper { get; set; }
        public int? ShipperID { get; set; }
        
        public Customer Customer { get; set; }
        public int CustomerID { get; set; }
        
        public List<OrderDetail> OrderDetails { get; set; }
    }
}