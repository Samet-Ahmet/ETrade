using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Dtos
{
    public class UsersRoleDto : IDto
    {
        public int UserId { get; set; }
        public string RoleName { get; set; }

    }
}
