using System.Collections.Generic;

namespace MarketPlaceCrm.WebApi.ViewModels.ProductsDtos
{
    public class EditProductForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> Images { get; set; }
        public List<StockEditForm> StockEditForms { get; set; }
    }

  
    public class StockEditForm
    {
        public int StockId { get; set; }
        public string Description { get; set; }

        public int Qty { get; set; }
        public int ProductId { get; set; }
    }
}