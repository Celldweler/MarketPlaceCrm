using System.Collections.Generic;
using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Enums;
using MarketPlaceCrm.Services.Models;

namespace MarketPlaceCrm.Services.OrderServices
{
    public class OrderHistoryService
    {
        private readonly AppDbContext _ctx;

        public OrderHistoryService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public List<OrderHistoryVm> GetOrderHistorySnapshot(int custid, int orderId)
        {
            var orderHistory = _ctx.OrderHistory
                .Where(x => x.CustomerId == custid && x.OrderId == orderId)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    CreatedOn = x.StatusUpdated.ToString("dd.MM.yyyy hh:mm"),
                    Status = x.Status switch
                    {
                        OrderStatuses.Pending => nameof(OrderStatuses.Pending),
                        OrderStatuses.Processing => nameof(OrderStatuses.Processing),
                        OrderStatuses.Paid => nameof(OrderStatuses.Paid),
                        OrderStatuses.Shipped => nameof(OrderStatuses.Shipped),
                        OrderStatuses.Delivered => nameof(OrderStatuses.Delivered),
                        OrderStatuses.Completed => nameof(OrderStatuses.Completed),
                        OrderStatuses.Rejected => nameof(OrderStatuses.Rejected),
                        _ => null
                    },
                    x.OrderId,
                    x.CustomerId
                })
                .ToArray();

            // var orderHistoryVm = new List<OrderHistoryVm>();
            //
            // for (int i = 0; i < orderHistory.Length; i++)
            // {
            //     orderHistoryVm.Add(new OrderHistoryVm
            //     {
            //         Id = orderHistory[i].Id,
            //         StatusUpdated = orderHistory[i].CreatedOn ,
            //         Status = orderHistory[i].Status switch
            //         {
            //             OrderStatuses.Pending => nameof(OrderStatuses.Pending),
            //             OrderStatuses.Processing => nameof(OrderStatuses.Processing),
            //             OrderStatuses.Paid => nameof(OrderStatuses.Paid),
            //             OrderStatuses.Shipped => nameof(OrderStatuses.Shipped),
            //             OrderStatuses.Delivered => nameof(OrderStatuses.Delivered),
            //             OrderStatuses.Completed => nameof(OrderStatuses.Completed),
            //             OrderStatuses.Rejected => nameof(OrderStatuses.Rejected),
            //             _ => null
            //         },
            //         OrderId = orderHistory[i].OrderId,
            //         CustomerId = orderHistory[i].CustomerId
            //     });
            // }
            
            return new List<OrderHistoryVm>();
        }
    }
}