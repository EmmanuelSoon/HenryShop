using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA1.Models
{
    public class ProductReview
    {
        public ProductReview()
        {
            Id = new Guid();
            CreatedDate = DateTime.Today.ToString("d");
        }
        public Guid Id { get; set; }
        public virtual Guid OrderId { get; set; }
        [Required]
        [MaxLength(250)]
        public string Content { get; set; }
        [Range(0, 5)]
        public int Rating { get; set; }
        public string CreatedDate { get; set; }


    }
}
