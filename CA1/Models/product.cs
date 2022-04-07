using System;
using Microsoft.EntityFrameworkCore;

namespace CA1.Models
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
        }
        
        public Guid Id { get; set; }
        public int prodId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Img { get; set; }
        public float Price { get; set; }

    }
}
