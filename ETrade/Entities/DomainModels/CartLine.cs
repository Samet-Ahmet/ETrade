using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace Entities.DomainModels
{
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
