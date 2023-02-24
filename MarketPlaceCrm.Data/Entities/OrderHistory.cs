using System;
using MarketPlaceCrm.Data.Enums;

namespace MarketPlaceCrm.Data.Entities
{
    public class OrderHistoryDetail
    {
        public int Id { get; set; }
        public DateTime StatusUpdated { get; set; } = DateTime.Now;
        public bool VisibleForCustomer { get; set; } = true;
        public OrderStatuses Status { get; set; }
        
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        public Order Order { get; set; }
        public Customer Customer { get; set; }
    }
}