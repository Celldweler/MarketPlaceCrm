using System;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;

namespace MarketPlaceCrm.WebApi.ViewModels.ProductsDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
    }

    public static class ProductVm
    {
        public static Expression<Func<Product, object>> Projection = y => new
        {
            y.Id,
            y.Name,
            y.Description,
            y.Cost,
            Created = y.Created.ToString("MM/dd/yyyy hh:mm"),
            y.Deleted,
            y.Image,
            Categories = y.Categories.Select(c => new { c.Category.Name})
            // Stocks = y.Stocks.Select(s => new
            // {
            //     s.Id,
            //     s.Description,
            //     s.Qty,
            //     Created = s.Created.ToString("MM/dd/yyyy hh:mm"),
            //     s.ProductId
            // }).ToList(),
        };

        public static Func<Product, object> MapToVm = Projection.Compile();
    }
}