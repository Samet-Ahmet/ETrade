using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Security.Hashing;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {

        private IWorkerService _workerService;
        private IGenderDal _genderDal;
        private IUserService _userService;
        private IAuthService _authService;

        public AdminController(IWorkerService workerService, IGenderDal genderDal, IUserService userService, IAuthService authService)
        {
            _workerService = workerService;
            _genderDal = genderDal;
            _userService = userService;
            _authService = authService;
        }

        [Authorize(Roles = "Manager,Worker")]
        public IActionResult Index()
        {
            
            return View();
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpGet]
        public IActionResult ManageAccount()
        {
            var email = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var result = _userService.GetByMail(email);

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            
            var result2 = _workerService.GetById(result.Data.Id);

            if (!result2.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result2.Message });
            }

            var model = new ManageAccountViewModel()
            {
                UpdateWorkerDto = new UpdateWorkerDto
                {
                    IdentityNo = result2.Data.IdentityNo,
                    FirstName = result.Data.FirstName,
                    LastName = result.Data.LastName,
                    Email = result.Data.Email,
                    Street = result2.Data.Street,
                    GenderId = result.Data.GenderId,
                    CityId = result2.Data.CityId,
                    PhoneNumber = result.Data.PhoneNumber,
                    DistrictId = result2.Data.DistrictId,
                    AddressNumber = result2.Data.AddressNumber,
                    BirthDate = result2.Data.BirthDate,
                    HireDate = result2.Data.HireDate
                },

                GenderNamesSelectItems = new List<SelectListItem>
                {
                    new SelectListItem()
                }
            };
            foreach (var gender in _genderDal.GetList())
            {
                model.GenderNamesSelectItems.Add(new SelectListItem
                {
                    Text = gender.GenderName,
                    Value = gender.GenderId.ToString()
                });
            }

            return View(model);
        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpPost]
        public IActionResult ManageAccount(UpdateWorkerDto updateWorkerDto)
        {
            var email = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var resultUser = _userService.GetByMail(email);

            if (!resultUser.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = resultUser.Message });
            }


            var resultManager = _workerService.GetById(resultUser.Data.Id);

            if (!resultManager.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = resultManager.Message });
            }

            var model = new ManageAccountViewModel()
            {
                UpdateWorkerDto = new UpdateWorkerDto
                {
                    IdentityNo = resultManager.Data.IdentityNo,
                    FirstName = resultUser.Data.FirstName,
                    LastName = resultUser.Data.LastName,
                    Email = resultUser.Data.Email,
                    Street = resultManager.Data.Street,
                    GenderId = resultUser.Data.GenderId,
                    CityId = resultManager.Data.CityId,
                    PhoneNumber = resultUser.Data.PhoneNumber,
                    DistrictId = resultManager.Data.DistrictId,
                    AddressNumber = resultManager.Data.AddressNumber,
                    BirthDate = resultManager.Data.BirthDate,
                    HireDate = resultManager.Data.HireDate
                },

                GenderNamesSelectItems = new List<SelectListItem>
                {
                    new SelectListItem()
                }
            };

            foreach (var gender in _genderDal.GetList())
            {
                model.GenderNamesSelectItems.Add(new SelectListItem
                {
                    Text = gender.GenderName,
                    Value = gender.GenderId.ToString()
                });
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userService.GetByMail(updateWorkerDto.Email);

            if (!user.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = user.Message });
            }

            user.Data.FirstName = updateWorkerDto.FirstName;
            user.Data.LastName = updateWorkerDto.LastName;
            user.Data.GenderId = updateWorkerDto.GenderId;
            user.Data.PhoneNumber = updateWorkerDto.PhoneNumber;

            var manager = _workerService.GetById(user.Data.Id);

            if (!manager.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = manager.Message });
            }

            manager.Data.Street = updateWorkerDto.Street;
            manager.Data.AddressNumber = updateWorkerDto.AddressNumber;
            manager.Data.CityId = updateWorkerDto.CityId;
            manager.Data.DistrictId = updateWorkerDto.DistrictId;

            var result = _workerService.Update(manager.Data, user.Data);

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new {errorMessage = result.Message});
            }

            TempData.Add(TempDataTypes.ManageInfo,Messages.UserUpdatedSuccessfully);

            return RedirectToAction("ManageAccount","Admin");

        }

        [Authorize(Roles = "Manager,Worker")]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value);
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [Authorize(Roles = "Manager,Worker")]
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
            return RedirectToAction("ChangePassword");
        }
    }
}
