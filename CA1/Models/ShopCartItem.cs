using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CA1.Models
{
    public class ShopCartItem
    {
        public ShopCartItem()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
        public virtual Guid ShopCartId { get; set; }
        //public bool IsFinished { get; set; }
    }
}
