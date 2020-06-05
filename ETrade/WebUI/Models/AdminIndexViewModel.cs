using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models
{
    public class AdminIndexViewModel
    {
        public List<Product> LowStock { get; set; }
        public List<Product> OutOfStock { get; set; }
    }
}
