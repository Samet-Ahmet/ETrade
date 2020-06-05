using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models
{
    public class OrderDetailsListViewModel
    {
        public List<OrderDetailDto> OrderDetails { get; set; }
        public int OrderId { get; set; }
    }
}
