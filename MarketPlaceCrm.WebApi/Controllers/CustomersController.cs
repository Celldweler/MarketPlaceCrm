using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Enums;
using MarketPlaceCrm.Services.OrderServices;
using MarketPlaceCrm.WebApi.ViewModels.CustomersDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/customers")]
    public class CustomersController : ApiBaseController
    {
        private readonly AppDbContext _ctx;
        private readonly OrderHistoryService _orderHistoryService;

        public CustomersController(AppDbContext ctx, OrderHistoryService orderHistoryService)
        {
            _ctx = ctx;
            _orderHistoryService = orderHistoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allCustomers = _ctx.Customers
                .Include(x => x.Orders)
                .ThenInclude(y => y.OrderDetails)
                .ThenInclude(x => x.Product)
                .Select(CustomerMapper.Projection)
                .ToList();

            return Ok(allCustomers);

            // return Ok(_ctx.ApplicationUsers.AsEnumerable());
        }

        [HttpGet("{custid}")]
        public IActionResult Get(int custid)
        {
            var customer = _ctx.Customers
                .Include(x => x.Orders)
                .Where(x => x.Id == custid)
                .Select(CustomerMapper.Projection)
                .FirstOrDefault();

            return Ok(customer);
        }


        [HttpGet("order-history/{custid}/{orderId}")]
        public IActionResult GetOrderHistory(int custid, int orderId)
        {
            // var orderHistoryVm = _orderHistoryService.GetOrderHistorySnapshot(custid, orderId);
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
                });
            
            return Ok(orderHistory);
        }
    }
}