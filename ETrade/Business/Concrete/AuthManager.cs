using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class AuthManager: IAuthService
    {
        private IUserDal _userDal;
        private IUserService _userService;
        private IUserRoleDal _userRoleDal;
    
        public AuthManager(IUserDal userDal, IUserService userService, IUserRoleDal userRoleDal)
        {
            _userDal = userDal;
            _userService = userService;
            _userRoleDal = userRoleDal;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            if (UserExists(userForRegisterDto.Email))
            {
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);
            }

            byte[] passwordSalt, passwordHash;

            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var user = new User
            {
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                Email = userForRegisterDto.Email,
                GenderId = userForRegisterDto.GenderId,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            try
            {
                _userDal.Add(user);
                _userRoleDal.Add(new UserRole
                {
                    RoleId = 1, //customer
                    UserId = _userService.GetByMail(user.Email).Data.Id
                });
                return new SuccessDataResult<User>(user);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<User>(Messages.ErrorWhileAddingEntity);
            }
            
            
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {

            if (!UserExists(userForLoginDto.Email))
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            var userToCheck = _userService.GetByMail(userForLoginDto.Email).Data;

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password,userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck);
        }

        public bool UserExists(string email)
        {
            if (!_userService.GetByMail(email).Success) //veri tabanında yoksa false
            {
                return false;
            }

            return true; //veri tabanında varsa result ture
        }


        public IDataResult<User> ChangePassword(User user, string newPassword)
        {
            byte[] passwordSalt, passwordHash;

            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            try
            {
                _userDal.Update(user);
                return new SuccessDataResult<User>(user);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<User>(Messages.ErrorWhileUpdatingEntity);
            }


        }
    }
}
