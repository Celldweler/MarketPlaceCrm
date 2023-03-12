using System.Collections.Generic;

namespace MarketPlaceCrm.WebApi.ViewModels.OrdersDtos
{
    public class OrderDto
    {
        public int ShippingMethodId { get; set; }
        public string ShippingMethod { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostCode { get; set; }
        
        public List<CartProductDto> CartProducts { get; set; }
    }

    public class CartProductDto
    {
        public int  ProductId { get; set; }
        public int  StockId { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
    }
}