namespace MarketPlaceCrm.Data.Entities
{
    public class Image : BaseEntity<int>
    {
        public string FileName { get; set; }

        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}