using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Gender : IEntity
    {
        public byte GenderId { get; set; }
        public string GenderName { get; set; }
    }
}
