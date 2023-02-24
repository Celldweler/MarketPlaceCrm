namespace MarketPlaceCrm.Services.Models
{
    public class OrderHistoryVm
    {
        public int Id { get; set; }
        public string StatusUpdated { get; set; } 
        public bool VisibleForCustomer { get; set; } = true;
        public string Status { get; set; }
        
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
    }
}