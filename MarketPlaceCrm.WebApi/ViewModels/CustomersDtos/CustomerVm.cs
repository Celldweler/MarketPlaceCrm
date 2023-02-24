using System;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.WebApi.Services;

namespace MarketPlaceCrm.WebApi.ViewModels.CustomersDtos
{
    public static class CustomerMapper
    {
        public static Expression<Func<Customer, object>> WithoutOrders = x =>
            new
            {
                x.Id,
                x.Name,
                x.LastName,
                x.Email,
                x.PhoneNumber,
                x.City,
                x.ZipCode,
                x.Address,
            };
        public static Expression<Func<Customer, object>> Projection = cust =>
            new
            {
                cust.Id,
                cust.Name,
                cust.LastName,
                cust.Email,
                cust.PhoneNumber,
                cust.City,
                cust.ZipCode,
                cust.Address,
                Orders = cust.Orders.Select(x => new
                {
                    x.Id,
                    Created = x.Created.Parse(),
                    x.Status,
                    x.CustomerID,
                    OrderDetails = x.OrderDetails.Select(o => new
                    {
                        o.Id,
                        o.OrderID,
                        o.UnitPrice,
                        o.Discount,
                        o.ProductID,
                        Product = new
                        {
                            o.Product.Id,
                            o.Product.Name,
                            o.Product.Description,
                            o.Product.Cost,
                            Created = o.Product.Created.Parse(),
                        },
                    })
                })
            };
    }
}