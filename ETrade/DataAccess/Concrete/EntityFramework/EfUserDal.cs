using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, ETradeContext>, IUserDal
    {
        public List<UsersRoleDto> GetRoles(User user)
        {
            using (var context = new ETradeContext())
            {
                var result = from role in context.Roles
                    join userRole in context.UserRoles
                        on role.RoleId equals userRole.RoleId
                    where userRole.UserId == user.Id
                    select new UsersRoleDto{ UserId = user.Id,  RoleName= role.RoleName };
                return result.ToList();
            }
        }
    }
}
