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
    public class AdminCategoryController : Controller
    {
        private ICategoryService _categoryService;

        public AdminCategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var result = _categoryService.GetAll();

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            var model = new CategoryListViewModel
            {
                Categories = result.Data
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var model = new AddCategoryViewModel
            {
                CategoryDto = new CategoryDto(),
                categories = new List<SelectListItem>()
            };

            var result = _categoryService.GetAll();

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            model.categories.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Ana Kategori"
            });

            foreach (var category in result.Data)
            {
                model.categories.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName,
                });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(CategoryDto categoryDto)
        {
            var model = new AddCategoryViewModel
            {
                CategoryDto = new CategoryDto(),
                categories = new List<SelectListItem>()
            };

            var result = _categoryService.GetAll();

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            model.categories.Add(new SelectListItem
            {
                Value = "-1",
                Text = "Ana Kategori"
            });

            foreach (var category in result.Data)
            {
                model.categories.Add(new SelectListItem
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName,
                });
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newCategory = new Category
            {
                CategoryName = categoryDto.Category.CategoryName,
                CategoryId = categoryDto.Category.CategoryId,
                IsMainCategory = categoryDto.MainCategoryId == -1 ? true : false
            };

            var result2 = _categoryService.AddCategory(newCategory, categoryDto.MainCategoryId);
            if (!result2.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result2.Message });
            }
            TempData.Add(TempDataTypes.CategoryInfo,Messages.CategoryAdded);
            return RedirectToAction("Index", "AdminCategory");
        }
        [HttpGet]
        public IActionResult Delete(int categoryId)
        {
            var category = _categoryService.GetByCategoryId(categoryId);

            if (!category.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = category.Message });
            }

            var result = _categoryService.DeleteCategory(category.Data);

            if (!result.Success)
            {
                if (result.Message.Equals(Messages.CategoryCantDeleted))
                {
                    TempData.Add(TempDataTypes.CategoryInfo,result.Message);
                    return RedirectToAction("Index", "AdminCategory");
                }
                else
                {
                    return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
                }
            }
            TempData.Add(TempDataTypes.CategoryDelete, Messages.CategoryDeleted);
            return RedirectToAction("Index", "AdminCategory");
        }
    }
}
