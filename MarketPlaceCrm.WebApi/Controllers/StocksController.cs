using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.WebApi.ViewModels.StocksDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/stocks")]
    public class StocksController : ApiBaseController
    {
        private readonly AppDbContext _ctx;

        public StocksController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var allStocks = _ctx.Stocks
                .Include(x => x.Product)
                .Select(StockVm.ProjectionIncludeProduct)
                .ToList();
            
            return Ok(allStocks);
        }
        
        [HttpGet("{stockId}")]
        public IActionResult Get(int stockId)
        {
            var stockByID = _ctx.Stocks
                .Include(x => x.Product)
                .Where(x => x.Id.Equals(stockId))
                .Select(StockVm.ProjectionIncludeProduct)
                .FirstOrDefault();
            
            return Ok(stockByID);
        }
        
        // create new stock
        
        // remove stock
        
        // update stock
        public class StockUpdateForm
        {
            public int Qty { get; set; }
        }
        [HttpPut("{stockId}/{productId}")]
        public IActionResult UpdateStock(int stockId, int productId, [FromForm]StockUpdateForm stockUpdateForm)
        {
            var stock = _ctx.Stocks.FirstOrDefault(x => x.Id == stockId && x.ProductId == productId);
            if (stock is null) return NoContent();

            // if (stockUpdateForm.Qty <= 0)
            //     return BadRequest("Invalid stock qty");
            var oldStockQty = stock.Qty;
            stock.Qty = stockUpdateForm.Qty;
            
            var backInStockSubscriptions = _ctx.BackInStockSubscriptions
                .Include(x => x.Product)
                .Include(y => y.Stock)
                .Where(x => x.ProductId == productId && x.StockId == stockId && x.IsAlreadyBackInStock == false)
                .ToList();
            if (backInStockSubscriptions.Count > 0 && oldStockQty == 0)
            {
                foreach (var subscription in backInStockSubscriptions)
                {
                    _ctx.Notifications.Add(new Notification
                    {
                        ReceiverId = subscription.CustomerId,
                        JsonData = $"The {subscription.Product.Name} {subscription.Stock.Description} Is Back In Stock",
                        IsRead = false,
                        Type = NotificationType.ProductBackInStock
                    });
                }
                
                _ctx.RemoveRange(backInStockSubscriptions);
            }

            _ctx.SaveChanges();
            
            return Ok();
        }
    }
}