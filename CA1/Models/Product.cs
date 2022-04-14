using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CA1.Models
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
            InventoryRecords = new List<InventoryRecord>();
            Orders = new List<Order>();
            ShopCartItems = new List<ShopCartItem>();
            WishListItems = new List<WishListItem>();
        }
        public Guid Id { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        public string Img { get; set; }
        public string DownLoadLink { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShopCartItem> ShopCartItems { get; set; }
        public virtual ICollection<InventoryRecord> InventoryRecords { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }

    }
}

