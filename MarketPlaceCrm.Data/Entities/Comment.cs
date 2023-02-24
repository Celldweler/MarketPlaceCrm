using System;
using System.Collections.Generic;
using MarketPlaceCrm.Data.Entities.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketPlaceCrm.Data.Entities
{

   
    public class Comment : VersionEntity
    {
        public int  Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;

        public bool PassedModeration { get; set; } 
        public bool Deleted { get; set; }

        public string SenderClientConnectionId { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
      
        // if comment is reply to another comment it has parent comment
        public Comment Parent { get; set; }
        public int ParentId { get; set; }

        // replies to review
        public List<Comment> Replies { get; set; }

        // who write this review
        public Customer From { get; set; }
        public int FromId { get; set; }
        
        // product where customer leave this comment
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}