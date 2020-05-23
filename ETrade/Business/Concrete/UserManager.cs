using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public IDataResult<User> GetByMail(string email)
        {
            try
            {
                return new SuccessDataResult<User>(_userDal.Get(u => u.Email == email));
            }
            catch (Exception e)
            {
                return new ErrorDataResult<User>();
            }
           
        }

        public IDataResult<List<UsersRoleDto>> GetRoles(User user)
        {
            try
            {
                return new SuccessDataResult<List<UsersRoleDto>>(_userDal.GetRoles(user));

            }
            catch (Exception e)
            {
               return new ErrorDataResult<List<UsersRoleDto>>();
            }
          
        }

        public IResult Add(User user)
        {
            try
            {
                _userDal.Add(user);
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult();
            }
        }

        public IResult Delete(User user)
        {

            try
            {
                _userDal.Delete(user);
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult();
            }
        }

        public IResult Update(User user)
        {
            try
            {
                _userDal.Update(user);
                return new SuccessResult();
            }
            catch (Exception e)
            {
                return new ErrorResult();
            }
        }
    }
}
