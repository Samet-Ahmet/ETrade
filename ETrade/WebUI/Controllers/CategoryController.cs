using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index(int categoryId = -1)
        {
            if (categoryId == -1)
            {
                var resultAll = _categoryService.GetMainCategories();

                if (resultAll.Success)
                {
                    var model = new CategoryListViewModel
                    {
                        Categories = resultAll.Data
                    };
                    return View(model);
                }
                return RedirectToAction("InternalError", "Error", new { errorMessage = resultAll.Message });
            }
            var resultSub = _categoryService.GetSubCategories(categoryId);

            if (resultSub.Success)
            {
                var model = new CategoryListViewModel
                {
                    Categories = resultSub.Data
                };
                return View(model);
            }

            if (resultSub.Message == Messages.ThereIsntSubCategory)
            {
                //productları listele
               // return RedirectToAction("Index", "Product", new { categoryId = result.Data.CategoryId });
                /* var result = _categoryService.GetMainCategory(categoryId);
                 return RedirectToAction("Index", "Category", new { categoryId = result.Data.CategoryId });*/
            }
            return RedirectToAction("InternalError", "Error", new { errorMessage = resultSub.Message });
        }

    }
}
