using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CA1.Models
{
    public class Order
    {
        public Order()
        {
            Id = new Guid();
            CreatedDate = DateTime.Today.ToString("d");
        }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        [Required]
        public string CreatedDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual Guid UserId {get; set;}
    }
}
