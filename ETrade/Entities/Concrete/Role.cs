using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class Role : IEntity
    {
        public byte RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
