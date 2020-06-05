using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class OrderDetailDto : IDto
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public short Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
