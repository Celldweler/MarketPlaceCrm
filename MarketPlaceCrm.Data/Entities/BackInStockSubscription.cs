using System;

namespace MarketPlaceCrm.Data.Entities
{
    public class BackInStockSubscription
    {
        public int  ID { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsAlreadyBackInStock { get; set; } 
        
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int StockId { get; set; }
        public Stock  Stock { get; set; }
    }
}