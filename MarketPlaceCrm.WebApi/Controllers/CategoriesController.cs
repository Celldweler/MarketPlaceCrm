using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : ApiBaseController
    {
        #region Fields

        private readonly AppDbContext _ctx;

        #endregion

        #region Ctor

        public CategoriesController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        #endregion

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_ctx.Categories.Select(x => new
            {
                x.Id,
                x.Name,
                x.ParentID
            }));
        }
        
        [HttpPut("{categoryId}/{productId}")]
        public IActionResult AddToCategory(int categoryId, int productId)
        {
            var checkIsAlreadyExist = _ctx.ProductCategoryRelationships
                .FirstOrDefault(x => x.ProductId == productId && x.CategoryId == categoryId);
            if (checkIsAlreadyExist != null)
                return BadRequest();
            
            _ctx.Add(new ProductCategoryRelationship
            {
                ProductId = productId,
                CategoryId = categoryId
            });
            var result = _ctx.SaveChanges();
            if(result > 0 ) return Ok();

            return BadRequest();
        }

        public class ChangeCategoryForm
        {
            public int OldCategoryId { get; set; }
            public int NewCategoryId { get; set; }

            public int ProductId { get; set; }
        }
        
        [HttpPut("changeCategoryForProduct")]
        public IActionResult Change([FromForm]ChangeCategoryForm changeCategoryForm)
        {
            // check is exist product
            var isProductExist = _ctx.Products.Any(x => x.Id.Equals(changeCategoryForm.ProductId));
            if (!isProductExist) return NotFound($"Product with Id: {changeCategoryForm.ProductId} not exist in database");
            
            // check is exist old category and new

            if (!_ctx.Categories.Any(x => x.Id.Equals(changeCategoryForm.OldCategoryId)) || 
                !_ctx.Categories.Any(x => x.Id.Equals(changeCategoryForm.NewCategoryId)))
                return NotFound("category not found");

            var prodCat = _ctx.ProductCategoryRelationships
                .FirstOrDefault(x => x.ProductId == changeCategoryForm.ProductId &&
                                     x.CategoryId == changeCategoryForm.OldCategoryId);
            if (prodCat is null)
                return NotFound();

            _ctx.ProductCategoryRelationships.Remove(prodCat);
            _ctx.ProductCategoryRelationships.Add(new ProductCategoryRelationship
            {
                ProductId = changeCategoryForm.ProductId,
                CategoryId = changeCategoryForm.NewCategoryId
            });
            _ctx.SaveChanges();
            
            return Ok("category successfully changed");
        }

        // get subs for certain category
        [HttpGet("{parentId}")]
        public IActionResult Get(int parentId)
        {
            return Ok(_ctx.Categories
                .Where(x => x.ParentID.Equals(parentId))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ParentID
                }));
        }
    }
}