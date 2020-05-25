using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using DataAccess.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private IWorkerService _workerService;
        private IGenderDal _genderDal;
        private ICityService _cityService;

        public UserController(IUserService userService, IWorkerService workerService, IGenderDal genderDal, ICityService cityService)
        {
            _userService = userService;
            _workerService = workerService;
            _genderDal = genderDal;
            _cityService = cityService;
        }

        public IActionResult Index()
        {
            var result = _workerService.GetAllManagers();
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }
            var model = new ManagerListViewModel
            {
                Managers = result.Data
            };
            return View(model);
        }

        public IActionResult Worker()
        {
            var result = _workerService.GetWorkers();
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }
            var model = new WorkerListViewModel
            {
                Workers = result.Data
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult AddManager()
        {
            var model = new AddManagerViewModel
            {
                Manager = new WorkerDetailsDto(),
                GenderNamesSelectItems = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Cinsiyet",Value = "0"}
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

        [HttpPost]
        public IActionResult AddManager(WorkerDetailsDto workerDetailsDto)
        {
            var model = new AddManagerViewModel
            {
                Manager = new WorkerDetailsDto(),
                GenderNamesSelectItems = new List<SelectListItem>
                {
                    new SelectListItem{Text = "Cinsiyet",Value = "0"}
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
            if (workerDetailsDto.GenderId == 0)
            {
                TempData.Add(TempDataTypes.GenderError, Messages.MustBeFilled);

                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetDistricts(int cityId)
        {
            var model = _cityService.GetDistrictsByCityId(cityId);

            return Json(model);
        }
    }
}
