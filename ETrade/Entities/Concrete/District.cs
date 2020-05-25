using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class District : IEntity
    {
        public byte CityId { get; set; }
        public short DistrictId { get; set; }
        public string DistrictName { get; set; }
    }
}
