using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IUserService
    {
        IDataResult<User> GetByMail(string email);
        IDataResult<List<UsersRoleDto>> GetRoles(User user);
        IResult Add(User user);
        IResult Delete(User user);
        IResult Update(User user);
        IDataResult<User> GetById(int userId);
    }
}
