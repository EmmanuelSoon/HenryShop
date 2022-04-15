using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CA1.Models
{
    public class WishListItem
    {
        public WishListItem()
        {
            Id = new Guid();
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        public Guid Id { get; set; }
        public virtual Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public virtual Guid WishListId { get; set; }
        public long TimeStamp { get; set; }
    }
}
