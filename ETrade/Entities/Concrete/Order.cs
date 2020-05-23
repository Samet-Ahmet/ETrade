using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
   public class Order : IEntity
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public int TrackingNumber { get; set; }
        public int ShippingDetailId { get; set; }
        public bool Delivered { get; set; }
        public decimal Price { get; set; }

    }
}
