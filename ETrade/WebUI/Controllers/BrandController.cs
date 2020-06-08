using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Manager,Worker")]
    public class BrandController : Controller
    {
        private IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        public IActionResult Index()
        {
            var result = _brandService.GetAll();

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            var model = new BrandListViewModel
            {
                Brands = result.Data
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddBrandViewModel
            {
                Brand = new Brand()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Brand brand)
        {
            var model = new AddBrandViewModel()
            {
                Brand = brand
            };

            if (brand.BrandName == null)
            {
                return View(model);
            }

            var result = _brandService.Add(brand);
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            TempData.Add(TempDataTypes.BrandInfo, Messages.BrandAdded);
            return RedirectToAction("Index", "Brand");
        }

        [HttpGet]
        public IActionResult Delete(int brandId)
        {
            var brand = _brandService.GetById(brandId);


            if (!brand.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = brand.Message });
            }

            var result = _brandService.Delete(brand.Data);

            if (!result.Success)
            {
                if (result.Message.Equals(Messages.BrandCantDeleted))
                {
                    TempData.Add(TempDataTypes.BrandCantDeleted, result.Message);
                    return RedirectToAction("Index", "Brand");
                }
                else
                {
                    return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
                }
            }
            TempData.Add(TempDataTypes.BrandDelete, Messages.BrandDeleted);
            return RedirectToAction("Index", "Brand");
        }
    }
}
