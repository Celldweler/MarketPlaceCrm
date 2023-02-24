using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.Data.Entities.Moderation;
using MarketPlaceCrm.Data.Infrastructure;
using MarketPlaceCrm.Data.SeedData;
using MarketPlaceCrm.WebApi.Services.FakeStores;
using MarketPlaceCrm.WebApi.ViewModels;
using MarketPlaceCrm.WebApi.ViewModels.ProductsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/products")]
    // [Authorize(Policy = Constants.AdminPolicy)]
    public class ProductsController : ApiBaseController
    {
        private readonly FakeProductStore _fakeProductStore;
        private readonly AppDbContext _ctx;

        public ProductsController(FakeProductStore fakeProductStore, AppDbContext ctx)
        {
            _fakeProductStore = fakeProductStore;
            _ctx = ctx;
        }

       

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var query = new Repository<Product>(_ctx)
                .Query(x => x.Stocks, x => x.Categories)
                .Select(ProductVm.Projection)
                .ToList();

            var products = _ctx.Products
                .Include(x => x.Stocks)
                .Select(ProductVm.Projection)
                .ToList();

            return Ok(query);
        }

        [HttpGet("related/{productId}")]
        public IActionResult RelatedProducts(int productId)
        {
            var product = _ctx.ProductCategoryRelationships
                // .Where(x => x.ProductId == productId)
                .AsEnumerable();
            
            return Ok(product);
        }

        [HttpGet("pages/{pageNumber}")]
        public IActionResult GetPagedList(int pageNumber)
        {
            // 2 product in 1 page
            var pageSize = 2;
            var count = _ctx.Products.Count();
            var result = _ctx.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsEnumerable();
                
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _ctx.Products
                .Include(x => x.Stocks)
                .Include(y => y.Categories)
                .Include(comment => comment.Reviews)
                .Where(x => x.Id.Equals(id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Cost,
                    x.Image,
                    Stocks = x.Stocks.Select(s => new { s.Id, s.Description, s.ProductId, s.Qty }),
                    Reviews = x.Reviews.AsQueryable().Select(CommentMapper.CommentDto).ToList(),
                    Categories = x.Categories.Select(c => new
                    {
                        c.CategoryId,
                        c.Category.Name,
                        c.ProductId,
                    })
                })
                .FirstOrDefault();

            return Ok(product);
        }

        [HttpGet("categories/{categoryID}")]
        public IActionResult FilterByCategory(int categoryID)
        {
            var productsFilteredByCategory = _ctx.Products
                .Include(x => x.Categories)
                .Where(x => x.Categories.Any(y => y.CategoryId == categoryID))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description,
                    x.Cost,
                    Categories = x.Categories.Select(c => new
                    {
                        c.CategoryId,
                        c.Category.Name,
                        c.ProductId,
                    })
                });
            return Ok(productsFilteredByCategory);
        }

        [HttpPost("{userId}")]
        // [Authorize(Policy = "mode")]
        public IActionResult Create([FromBody] CreateProductForm createProductForm, int userId)
        {
            if (CheckForNull(createProductForm)) return BadRequest("invalid form");
            if (userId <= 0) return BadRequest("userId <= 0");
            
            var newProduct = new Product()
            {
                Name = createProductForm.Name,
                Cost = createProductForm.Price,
                Description = createProductForm.Description,
                Image = createProductForm.ImageFileName,
                IsPublished = false,
                Stocks = createProductForm.Stocks.Select(x => new Stock
                {
                    Description = x.Description,
                    Qty = x.Qty
                }).ToList(),
            };


            try
            {
                _ctx.Add(newProduct);
                _ctx.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("failed during added product to database");
            }
            finally
            {
                _ctx.Add(new ModerationItem
                {
                    Type = ModerationTypes.NewAddedProduct,
                    UserId = userId, // item created by user {userId}
                });
                _ctx.SaveChanges();
            }

            return Ok(ProductVm.MapToVm(newProduct));
        }

        [HttpDelete("{productId}")]
        public IActionResult Remove(int productId)
        {
            var product = _ctx.Products.FirstOrDefault(x => x.Id.Equals(productId));
            _ctx.Remove(product);
            var result = _ctx.SaveChanges();

            return Ok();
        }

        public static Expression<Func<EditProductForm, Product>> MapFromVmToEntity = p =>
            new Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
            };

        [HttpPut]
        public IActionResult Update([FromBody] EditProductForm editProductForm)
        {
            var isAlreadyExist = _ctx.Products.Any(x => x.Id == editProductForm.Id);
            if (!isAlreadyExist) return NotFound($"product with id: {editProductForm.Id} not found");

            var productToEdit = new Product
            {
                Id = editProductForm.Id,
                Name = editProductForm.Name,
                Description = editProductForm.Description,
            };
            _ctx.Attach(productToEdit);
            // _ctx.Entry(productToEdit).State = EntityState.Modified;
            _ctx.Update(productToEdit);
            _ctx.SaveChanges();
          
            throw new NotImplementedException();
        }
    }
}