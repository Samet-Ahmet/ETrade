using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;
        private IGenderDal _genderDal;

        public AccountController(IAuthService authService, IUserService userService, IGenderDal genderDal)
        {
            _authService = authService;
            _userService = userService;
            _genderDal = genderDal;
        }


        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel
            {
                UserForLoginDto = new UserForLoginDto()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var model = new LoginViewModel
            {
                UserForLoginDto = new UserForLoginDto()
            };

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = _authService.Login(userForLoginDto);
            if (!result.Success)
            {
                TempData.Add(TempDataTypes.LoginError, Messages.IncorrectEmailOrPassword);
                return View(model);
            }

            var roles = _userService.GetRoles(result.Data);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userForLoginDto.Email),
                new Claim(ClaimTypes.Name,result.Data.FirstName + " " + result.Data.LastName)
            };
            bool isManagerOrWorker = false;
            foreach (var role in roles.Data)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
                if (role.RoleName.Equals("Manager") || role.RoleName.Equals("Worker"))
                {
                    isManagerOrWorker = true;
                }
            }

            var userIdentity = new ClaimsIdentity(claims, "login");
            HttpContext.Session.SetString("Role",roles.Data.First().RoleName);

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(principal);

            HttpContext.Session.SetString("login", "yes"); //login oldu mu? kontrol için
            
            if (isManagerOrWorker)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                UserForRegisterDto = new UserForRegisterDto(),
                GenderNamesSelectItems = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Cinsiyet",Value = "0"}
                }
            };

           // model.GenderNames.Add(new SelectListItem("Cinsiyet","0"));
            foreach (var gender in _genderDal.GetList())
            {
                model.GenderNamesSelectItems.Add(new SelectListItem
                {
                    Text = gender.GenderName, Value = gender.GenderId.ToString()
                });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var model = new RegisterViewModel
            {
                UserForRegisterDto = new UserForRegisterDto(),
                GenderNamesSelectItems = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Cinsiyet",Value = "0"}
                }
            };

            // model.GenderNames.Add(new SelectListItem("Cinsiyet","0"));
            foreach (var gender in _genderDal.GetList())
            {
                model.GenderNamesSelectItems.Add(new SelectListItem
                {
                    Text = gender.GenderName,
                    Value = gender.GenderId.ToString()
                });
            }
            if (userForRegisterDto.GenderId == 0)
            {
                TempData.Add(TempDataTypes.GenderError, Messages.MustBeFilled);
                
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //model geçerli 
            
            if (!userForRegisterDto.Password.Equals(userForRegisterDto.PasswordConfirm))
            {
                TempData.Add(TempDataTypes.PasswordConfirm, "Şifreler uyuşmuyor, lütfen tekrar deneyiniz");
                return View(model);
            }
            //şifreler uyuştu
           // userForRegisterDto.GenderId = _genderDal.Get(g => g.GenderName.Equals(userForRegisterDto.GenderName)).GenderId;
            var result = _authService.Register(userForRegisterDto, userForRegisterDto.Password);

            if (!result.Success)
            {
                TempData.Add(TempDataTypes.RegisterError, result.Message);
                return View(model);
            }

            return RedirectToAction("RegisteredSuccessfully", "Account",new {userForRegisterDto = userForRegisterDto});
            //   return RegisteredSuccessfully(userForRegisterDto);
        }

        public IActionResult RegisteredSuccessfully(UserForRegisterDto userForRegisterDto)
        {
            var model = new RegisterViewModel
            {
                UserForRegisterDto = userForRegisterDto
            };

            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("login");
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Index", "Product");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var model = new ChangePasswordViewModel();
            return View(model);
        }
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var checkOldPassword = HashingHelper.VerifyPasswordHash(changePasswordViewModel.OldPassword, user.Data.PasswordHash,
                user.Data.PasswordSalt);
            if (!checkOldPassword)
            {
                TempData.Add(TempDataTypes.OldPasswordCheck, Messages.OldPasswordIncorrect);
                return View();
            }

            var result = _authService.ChangePassword(user.Data, changePasswordViewModel.NewPassword);
            if (!result.Success)
            {
                TempData.Add(TempDataTypes.ManageInfo, result.Message);
                return View();
            }

            TempData.Add(TempDataTypes.ManageInfo, Messages.PasswordChanged);
            return RedirectToAction("Manage");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult ManageProfile()
        {
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var model = new ManageProfileViewModel
            {
                User = user.Data,
                GenderNamesSelectItems = new List<SelectListItem>()
            };

            foreach (var gender in _genderDal.GetList())
            {
                model.GenderNamesSelectItems.Add(new SelectListItem
                {
                    Text = gender.GenderName,
                    Value = gender.GenderId.ToString()
                });
            }

            model.GenderNamesSelectItems.SingleOrDefault(g => g.Value.Equals(user.Data.GenderId.ToString())).Selected = true;


            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> ManageProfile(ManageProfileViewModel manageProfileViewModel)
        {
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var model = new ManageProfileViewModel
            {
                User = user.Data,
                GenderNamesSelectItems = new List<SelectListItem>()
            };

            foreach (var gender in _genderDal.GetList())
            {
                model.GenderNamesSelectItems.Add(new SelectListItem
                {
                    Text = gender.GenderName,
                    Value = gender.GenderId.ToString()
                });
            }

            model.GenderNamesSelectItems.SingleOrDefault(g => g.Value.Equals(user.Data.GenderId.ToString())).Selected = true;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            user.Data.FirstName = manageProfileViewModel.User.FirstName;
            user.Data.LastName = manageProfileViewModel.User.LastName;
            user.Data.PhoneNumber = manageProfileViewModel.User.PhoneNumber;
            user.Data.GenderId = manageProfileViewModel.User.GenderId;

            var result = _userService.Update(user.Data);

            if (!result.Success)
            {
                TempData.Add(TempDataTypes.ManageUpdateError,result.Message);
                return View(model);
            }

            await HttpContext.SignOutAsync();

            var roles = _userService.GetRoles(user.Data);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Data.Email),
                new Claim(ClaimTypes.Name,user.Data.FirstName + " " + user.Data.LastName)
            };

            foreach (var role in roles.Data)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(principal);

            TempData.Add(TempDataTypes.ManageInfo,Messages.UserUpdatedSuccessfully);

            return RedirectToAction("Manage", "Account");
        }
    }

}
