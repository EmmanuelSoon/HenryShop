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

        public ShopCartItem(Product product)
        {
            ProductId = product.Id;
            Id = new Guid();
        }

        public Guid Id { get; set; }

        public int Quantity { get; set; }
        public Guid ProductId { get; set; }

        [ForeignKey ("ProductId")]
        public virtual Product Product { get; set; }

        public virtual Guid ShopCartId { get; set; }
    }
}
