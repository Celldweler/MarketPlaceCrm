using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
                    JsonData =
                        $"{currentModerator.Email} changed order current order #{changeOrderStatusForm.OrderId}, status: {changeOrderStatusForm.Status.ToString()}",
                });
            }

            _ctx.SaveChanges();

            return Ok();
        }

        public class CheckoutForm
        {
            public int ProductId { get; set; }
            public int StockId { get; set; }
            public int Qty { get; set; }
            public decimal Price { get; set; }


            public List<CartProduct> CartProducts { get; set; }
        }

        public class CartProduct
        {
            public int ProductId { get; set; }

            public int StockId { get; set; }

            // public int CustomerId { get; set; }
            public int Qty { get; set; }
            public decimal Price { get; set; }
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromForm] CheckoutForm checkoutForm)
        {
            // TODO: only if customer ordered 1 product
            if (checkoutForm == null) return BadRequest();

            var checkInStock = _ctx.Stocks.Include(x => x.Product)
                .FirstOrDefault(x => x.Id == checkoutForm.StockId && x.ProductId == checkoutForm.ProductId);
            if (checkInStock == null)
                return BadRequest($"{checkInStock.Product.Name} {checkInStock.Description} sold out");

            if (checkoutForm.Qty <= 0) return BadRequest("invalid qty of product");
            checkInStock.Qty -= checkoutForm.Qty;

            var customerEmail = HttpContext.User?.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value;
            var customer = _ctx.Customers.FirstOrDefault(x => x.Email.Equals(customerEmail));

            if (customer == null) return BadRequest($"customer {customerEmail} not exist in db");

            var newOrder = new Order
            {
                CustomerID = customer.Id,
                Status = OrderStatuses.Pending,
                Deleted = false,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Qty = checkoutForm.Qty,
                        ProductID = checkoutForm.ProductId,
                        StockId = checkoutForm.StockId,
                        UnitPrice = checkoutForm.Price
                    }
                }
                // OrderDetails = checkoutForm.CartProducts.Select(x => new OrderDetail
                // {
                //     Qty = x.Qty,
                //     ProductID = x.ProductId,
                //     StockId = x.StockId,
                //     UnitPrice = x.Price
                // }).ToList()
            };

            await _ctx.AddAsync(newOrder);
            await _ctx.SaveChangesAsync();

            return Ok(newOrder.Id);
        }

        // new orders pending processing
    }
}