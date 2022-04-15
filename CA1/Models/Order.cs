using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA1.Models
{
    public class Order
    {
        public Order()
        {
            Id = new Guid();
            CreatedDate = DateTime.Now.ToString("MM-dd-yyyy HH:mm");
            OrderDetails = new List<OrderDetail>();
            ProductReview = null;
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string CreatedDate { get; set; }
        public long TimeStamp { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public virtual ProductReview ProductReview { get; set; }
        public virtual Guid UserId {get; set;}
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
