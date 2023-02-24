using System;
using System.Linq;
using System.Linq.Expressions;
using MarketPlaceCrm.Data.Entities;
using MarketPlaceCrm.WebApi.Services;

namespace MarketPlaceCrm.WebApi.ViewModels
{
    public static class CommentMapper
    {
        public static Expression<Func<Comment, object>> CommentDto = v =>
            new
            {
                v.Id,
                v.Text,
                v.PassedModeration,
                v.Deleted,
                Created = v.Created.Parse(),
                From = v.From.Name + " " + v.From.LastName,
                v.Rating,
                v.Likes,
                v.Dislikes,
                Replies = v.Replies.Select(reply => reply.Text),
                v.ProductId,
                v.Product.Name
            };
    }
}