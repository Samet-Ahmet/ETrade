using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class City : IEntity
    {
        public byte CityId { get; set; }
        public string CityName { get; set; }

    }
}
