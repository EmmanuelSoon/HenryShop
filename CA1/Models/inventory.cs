using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CA1.Models
{
    public class Inventory
    {
        public Inventory()
        {

            Id = new Guid();
            ActivCodes = new List<ActivCode>();
        }
        
        public Guid Id { get; set; }
        public int productid { get; set; }
        public int qty { get; set; }
      
        public virtual ICollection<ActivCode> ActivCodes { get; set; }
    }
}
