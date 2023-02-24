using System;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Enums;
using MarketPlaceCrm.WebApi.ViewModels.CustomersDtos;

namespace MarketPlaceCrm.WebApi.ViewModels
{
    public static class OrderVm
    {
        public static Func<Order, object> Projection = x =>
            new
            {
                x.Id,
                Status = x.Status switch
                {
                    OrderStatuses.New => nameof(OrderStatuses.New),
                    OrderStatuses.Pending => nameof(OrderStatuses.Pending),
                    OrderStatuses.Processing => nameof(OrderStatuses.Processing),
                    OrderStatuses.Paid => nameof(OrderStatuses.Paid),
                    OrderStatuses.Shipped => nameof(OrderStatuses.Shipped),
                    OrderStatuses.Delivered => nameof(OrderStatuses.Delivered),
                    OrderStatuses.Completed => nameof(OrderStatuses.Completed),
                    OrderStatuses.Rejected => nameof(OrderStatuses.Rejected),
                },
                x.ShipAddress,
                x.ShipCity,
                x.ShipCountry,
                x.ShipRegion,
                x.ShipZipCode,
                x.Freight,
                x.CustomerID,
                Created = x.Created.ToString("dd.mm.yyyy hh:mm"),
                OrderDetails = x.OrderDetails.AsQueryable().Select(OrderDetailMapper.Projection),
                Customer = CustomerMapper.WithoutOrders.Compile().Invoke(x.Customer)
            };
    }
}