using System;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;

namespace MarketPlaceCrm.WebApi.ViewModels.StocksDtos
{
    public static class StockVm
    {
        public static Expression<Func<Stock, object>> ProjectionIncludeProduct = (s) =>
            new
            {
                s.Id,
                s.Description,
                s.Qty,
                Created = s.Created.ToString("M/d/yy hh:mm"),
                s.ProductId,
                s.Deleted,
                Product = new
                {
                    s.Product.Id,
                    s.Product.Name,
                    s.Product.Description,
                    s.Product.Cost,
                    Created = s.Product.Created.ToString("M/d/yy hh:mm"),
                }
            };
        public static Expression<Func<Stock, object>> ProjectionWithoutProduct = (s) =>
            new
            {
                s.Id,
                s.Description,
                s.Qty,
                Created = s.Created.ToString("M/d/yy hh:mm"),
                s.ProductId,
                s.Deleted,
            };
    }
}