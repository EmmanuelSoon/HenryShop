using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA1.Models
{
    public class ShopCart
    {
        public ShopCart()
        {
            ShopCartItems = new List<ShopCartItem>();
            Id = new Guid();
            Created = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        }
        public Guid Id { get; set; }
        public long Created { get; set; }

        public virtual ICollection<ShopCartItem> ShopCartItems { get; set; }
        public virtual Guid? UserId { get; set; }
    }
}
