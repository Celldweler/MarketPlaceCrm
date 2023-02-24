namespace MarketPlaceCrm.Data.Entities
{
    public class OrderDetail 
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; } // => Qty * UnitPrice
        public decimal Discount { get; set; }
        
        public Product? Product { get; set; }
        public int? ProductID { get; set; }  
        
        public Order Order { get; set; }
        public int OrderID { get; set; }
    }
}