using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            if (LoginUser(userForLogin.Email, userForLogin.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userForLogin.Email),
                    new Claim(ClaimTypes.Role,"Admin")
                };

                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);
                HttpContext.Session.SetString("login", "yes");

                //Just redirect to our index after logging in. 
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("login");
            return RedirectToAction("Index", "Home");
        }

        private bool LoginUser(string username, string password)
        {
            if (username == "ahmetkkn07@gmail.com" && password == "1")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
