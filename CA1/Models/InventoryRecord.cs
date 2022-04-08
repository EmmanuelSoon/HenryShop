using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CA1.Models
{
    public class InventoryRecord
    {
        public InventoryRecord()
        {
            Id = new Guid();
            IsUsed = false;
        }
        public Guid Id { get; set; }
        public bool IsUsed { get; set; }
        public Guid ActivationId { get; set; }
        public virtual Guid ProductId { get; set; }

    }
}
