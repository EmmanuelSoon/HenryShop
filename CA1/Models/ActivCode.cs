using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CA1.Models
{
    public class ActivCode
    {   
        public Guid Id { get; set; }

        public int productid;

        public List<Guid> activationCode;

    }
}
