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
        }
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Price is required.")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Name of product is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description of product is required.")]
        public string Desc { get; set; }
        [Required(ErrorMessage = "Image of product is required.")]
        public string Img { get; set; }
        public string DownLoadLink { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShopCartItem> ShopCartItems { get; set; }
        public virtual ICollection<InventoryRecord> InventoryRecords { get; set; }



    }
}

