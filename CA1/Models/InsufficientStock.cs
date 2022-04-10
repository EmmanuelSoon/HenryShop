using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA1.Models
{
    public class InsufficientStock
    {
        public InsufficientStock()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }

        public int Quantity { get; set; }
        public virtual Guid ShopCartItemId {get;set;}
        [ForeignKey("ShopCartItemId")]
        public virtual ShopCartItem ShopCartItem { get; set; }


    }
}
