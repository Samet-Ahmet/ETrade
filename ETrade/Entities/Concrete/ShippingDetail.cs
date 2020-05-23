using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class ShippingDetail : IEntity
    {
        public int ShippingDetailId { get; set; }
        public string AddressName { get; set; }
        public int CustomerId { get; set; }
        public byte CityId { get; set; }
        public byte DistrictId { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }

    }
}
