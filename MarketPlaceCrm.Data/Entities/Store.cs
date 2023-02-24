using System;
using System.Collections.Generic;

namespace MarketPlaceCrm.Data.Entities
{
    public class Store : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoPath { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        // график работы
        public string Schedule { get; set; }
        
        public string OwnerId { get; set; }
        public List<string>? Staffs { get; set; }
        public List<Product> Products { get; set; }
        public int Rate { get; set; }
        public List<string> CustomersReviews { get; set; }
    }
}