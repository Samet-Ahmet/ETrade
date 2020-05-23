using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;

        public AccountController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
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
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var result = _authService.Login(userForLogin);
            if (!result.Success)
            {
                return View();
            }

            var roles = _userService.GetRoles(result.Data);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userForLogin.Email)
            };

            foreach (var role in roles.Data)
            {
                claims.Add(new Claim(ClaimTypes.Role,role.RoleName));
            }

            var userIdentity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(principal);

            HttpContext.Session.SetString("login", "yes"); //login oldu mu? kontrol için

            return RedirectToAction("Index2", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                UserForRegisterDto = new UserForRegisterDto()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.GenderId = 1;
           var result = _authService.Register(userForRegisterDto, userForRegisterDto.Password);

           if (!result.Success)
           {
               return View();
           }

           return RedirectToAction("RegisteredSuccessfully","Account");
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
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
