using System;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;

namespace MarketPlaceCrm.WebApi.ViewModels
{
    public static class OrderDetailMapper
    {
        public static Expression<Func<OrderDetail, object>> Projection = o =>
            new
            {
                o.Id,
                o.Qty,
                o.Total,
                o.UnitPrice,
                o.Discount,
                o.ProductID,
                o.OrderID,
                o.Product
            };
    }
}