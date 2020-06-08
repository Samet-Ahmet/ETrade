using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;

namespace WebUI.Models
{
    public class OrderIndexViewModel
    {
        public List<Order> Orders { get; set; }
        public bool Control { get; set; }
    }
}
