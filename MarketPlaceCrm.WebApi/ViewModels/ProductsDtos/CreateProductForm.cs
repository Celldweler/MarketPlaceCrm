using System.Collections;
using System.Collections.Generic;

namespace MarketPlaceCrm.WebApi.ViewModels.ProductsDtos
{
    public class CreateProductForm
    {
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<StockForm> Stocks { get; set; }
    }

    public class StockForm
    {
        public int  StockId { get; set; }
        public int ProductId { get; set; }
        
        public string Description { get; set; }
        public int Qty { get; set; }
    }
}