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

        }
        public Guid Id { get; set; }
        public Guid ActivationId { get; set; }
        public virtual Guid ProductId { get; set; }

    }
}
