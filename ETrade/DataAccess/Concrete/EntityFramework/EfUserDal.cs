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
                var result = from r in context.Roles
                    join ur in context.UserRoles on r.RoleId equals ur.RoleId
                    join u in context.Users on ur.UserId equals u.Id
                             where u.Id==user.Id
                    select
                        new UsersRoleDto
                        {
                            RoleName = r.RoleName,
                            UserId = u.Id
                        };
                return result.ToList();
            }
        }
    }
}
