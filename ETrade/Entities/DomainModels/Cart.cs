using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DomainModels
{
    public class Cart
    {
        public Cart()
        {
            //samete sor 
            //neden list instance değil?
            CartLines = new List<CartLine>();
        }
        public List<CartLine> CartLines { get; set; }
    }
}
