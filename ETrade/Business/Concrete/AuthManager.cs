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
    
        public AuthManager(IUserDal userDal, IUserService userService)
        {
            _userDal = userDal;
            _userService = userService;
        }


        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            if (UserExists(userForRegisterDto.Email).Success)
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
                return new SuccessDataResult<User>(user);
            }
            catch (Exception e)
            {
                return new ErrorDataResult<User>(Messages.Error);
            }
            
            
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {

            if (UserExists(userForLoginDto.Email).Success)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            var result = _userService.GetByMail(userForLoginDto.Email);
            User userToCheck;
            if (!result.Success)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            userToCheck = result.Data;
            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password,userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null) //veri tabanında yoksa 
            {
                 return new ErrorResult(Messages.UserAlreadyExists); //result false
            }

            return new SuccessResult(); //veri tabanında varsa result true
        }
    }
}
