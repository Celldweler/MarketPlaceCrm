using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Entities.Moderation;
using MarketPlaceCrm.Data.Enums;
using MarketPlaceCrm.WebApi.ViewModels;
using MarketPlaceCrm.WebApi.ViewModels.CustomersDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/orders")]
    public class OrdersController : ApiBaseController
    {
        private readonly AppDbContext _ctx;

        public OrdersController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        
        // get all orders for all time
        [HttpGet]
        public IActionResult Get()
        {
            var orders = _ctx.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Where(x => !x.Deleted && x.VersionState == VersionState.Live)
                .OrderByDescending(x => x.Created)
                .AsEnumerable()
                .Select(OrderVm.Projection);
                // .Select(x => new
                // {
                //     x.Id,
                //     x.Status,
                //     x.ShipAddress,
                //     x.ShipCity,
                //     x.ShipCountry,
                //     x.ShipRegion,
                //     x.ShipZipCode,
                //     x.Freight,
                //     x.CustomerID,
                //     Created = x.Created.ToString("dd.mm.yyyy hh:mm"),
                //     OrderDetails = x.OrderDetails.AsQueryable().Select(OrderDetailMapper.Projection),
                //     Customer = CustomerMapper.WithoutOrders.Compile().Invoke(x.Customer)
                // })
            
            return Ok(orders);
        }

        [HttpGet("latest")]
        public IActionResult GetLatest()
        {
            var latestOrders = _ctx.Orders
                .Include(x => x.Customer)
                .Include(x => x.OrderDetails)
                .Where(x => x.Status != OrderStatuses.Completed && x.Status != OrderStatuses.Rejected)
                .OrderByDescending(x => x.Created)
                .AsEnumerable()
                .Select(OrderVm.Projection);
            
            return Ok(latestOrders);
        }

        [HttpGet("{orderId}")]
        public IActionResult GetFullOrderInfo(int orderId)
        {
            return Ok();
        }
        public class ChangeOrderStatusForm
        {
            public OrderStatuses Status { get; set; }
            public int OrderId { get; set; }
            public int ModeratorId { get; set; }
        }
        [HttpPost("changeStatus")]
        public IActionResult ChangeOrderStatus([FromForm] ChangeOrderStatusForm changeOrderStatusForm)
        {
            if (CheckForNull(changeOrderStatusForm))
                return BadRequest();

            var order = _ctx.Orders.FirstOrDefault(x => x.Id == changeOrderStatusForm.OrderId);
            if (order is null) return NotFound();


            // var tempVersionOrder = new Order
            // {
            //     Deleted = order.Deleted,
            //     Created = order.Created,
            //     Status = order.Status,
            //     CustomerID = order.CustomerID,
            //     ShipAddress = order.ShipAddress,
            //     VersionState = VersionState.Staged,
            // };
            // _ctx.Orders.Add(tempVersionOrder);
            
            _ctx.OrderHistory.Add(new OrderHistoryDetail
            {
                OrderId = changeOrderStatusForm.OrderId,
                CustomerId = order.CustomerID,
                Status = changeOrderStatusForm.Status
            });
            _ctx.SaveChanges();
            
            order.Status = changeOrderStatusForm.Status;
            
            _ctx.ModerationItems.Add(new ModerationItem
            {
                // CurrentId = tempVersionOrder.Id,
                TargetId = order.Id,
                Type = ModerationTypes.Order,
                IsRejected = false,
                UserId = changeOrderStatusForm.ModeratorId
            });

            var currentModerator = _ctx.ApplicationUsers.FirstOrDefault(x => x.Id == changeOrderStatusForm.ModeratorId);
            var moderators = _ctx.ApplicationUsers
                .Where(x => x.Role == UserRoles.Moderator).AsEnumerable();
            foreach (var mod in moderators)
            {
                _ctx.Notifications.Add(new Notification
                {
                    SenderId = changeOrderStatusForm.ModeratorId,
                    ReceiverId = mod.Id,
                    IsRead = false,
                    Type = NotificationType.ChangeOrderStatus,
                    JsonData = $"{currentModerator.Email} changed order current order #{changeOrderStatusForm.OrderId}, status: {changeOrderStatusForm.Status.ToString()}",
                });
            }
            _ctx.SaveChanges();

            return Ok();
        }
        
        // new orders pending processing
    }
}