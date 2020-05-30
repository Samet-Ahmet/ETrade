using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.DomainModels;

namespace WebUI.Models
{
    public class CompleteOrderViewModel
    {
        public ShippingDetail ShippingDetail { get; set; }
        public Cart Cart { get; set; }

    }
}
