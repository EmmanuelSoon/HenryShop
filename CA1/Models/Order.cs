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
            CreatedDate = DateTime.Today.ToString("d");
            OrderDetails = new List<OrderDetail>();
        }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        [Required]
        public string CreatedDate { get; set; }


        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual Guid UserId {get; set;}
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
