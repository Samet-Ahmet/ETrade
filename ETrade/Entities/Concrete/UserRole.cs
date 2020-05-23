using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class UserRole : IEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
