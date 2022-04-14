using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CA1.Models
{
    public class WishList
    {
        public WishList()
        {
            Id = new Guid();
            WishListItems = new List<WishListItem>();
        }
        public Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual ICollection<WishListItem> WishListItems { get; set; }
    }
}
