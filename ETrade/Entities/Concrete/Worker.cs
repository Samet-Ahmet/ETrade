using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Worker : IEntity
    {
        public int WorkerId { get; set; }
        public int IdentityNo { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public byte CityId { get; set; }
        public byte DistrictId { get; set; }
        public string Street { get; set; }
        public string AddressNumber { get; set; }
        public DateTime QuitDate { get; set; }
    }
}
