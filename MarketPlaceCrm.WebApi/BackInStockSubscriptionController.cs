using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceCrm.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackInStockSubscriptionController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public BackInStockSubscriptionController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ctx.BackInStockSubscriptions.AsEnumerable());
        }
        
        public class BackInStockSubscriptionDto
        {
            public int ProductId { get; set; }
            public int StockId { get; set; }
            public int CustomerId { get; set; }
        }
        
        [HttpPost]
        public IActionResult Create([FromForm] BackInStockSubscriptionDto backInStockSubscriptionDto)
        {
            var backInStockSubscription = new BackInStockSubscription
            {
                ProductId = backInStockSubscriptionDto.ProductId,
                StockId = backInStockSubscriptionDto.StockId,
                CustomerId = backInStockSubscriptionDto.CustomerId,
                IsAlreadyBackInStock = false
            };
            _ctx.Add(backInStockSubscription);
            _ctx.SaveChanges();
            
            return Ok(backInStockSubscription);
        }
    }
}