using System;
using System.Linq;
using MarketPlaceCrm.Data.Context;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.WebApi.Services;
using MarketPlaceCrm.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCrm.WebApi.Controllers
{
    [Route("api/comments")]
    public class CommentsController : ApiBaseController
    {
        private readonly AppDbContext _ctx;

        public CommentsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        // get comments for customer
        // [Authorize]
        [HttpGet("all")]
        public IActionResult CustomerComments()
        {
            // var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var comments = _ctx.Comments
                .Include(x => x.From)
                .Include(p => p.Product)
                .Select(CommentMapper.CommentDto)
                .AsEnumerable();
            
            return Ok(comments);
        }
        
        // get all comments for product
        [HttpGet("{productId}")]
        public IActionResult ReviewsForProduct(int productId)
        {
            
            
            return Ok();
        }

        [HttpGet("passedModeration")]
        public IActionResult AllComments()
        {
            var comments = _ctx.Comments
                .Include(x => x.From)
                .Include(p => p.Product)
                .Where(c => !c.Deleted && c.PassedModeration)
                .Select(CommentMapper.CommentDto)
                .AsEnumerable();
            
            return Ok(comments);
        }
        
        public class CreateCommentForm
        {
            public int CustomerId { get; set; }
            public string Text { get; set; }
            public int ProductId { get; set; }
            public string ConnectionId { get; set; }
        }
        
        // create
        [HttpPost]
        public IActionResult Create([FromBody] CreateCommentForm commentForm)
        {
            if (commentForm == null) return BadRequest();
            
            _ctx.Add(new Comment
            {
                Text = commentForm.Text,
                Created = DateTime.Now,
                ProductId = commentForm.ProductId,
                FromId = commentForm.CustomerId,
                SenderClientConnectionId = commentForm.ConnectionId,
            });
            
            _ctx.SaveChanges(); 
            
            return Ok();
        }
        // update 
        
        // delete
    }
}