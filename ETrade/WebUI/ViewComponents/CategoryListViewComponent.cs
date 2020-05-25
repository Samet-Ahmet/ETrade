using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebUI.Models;

namespace WebUI.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke(int categoryId = -1)
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
              //  return RedirectToAction("InternalError", "Error", new { errorMessage = resultAll.Message });
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
           // return RedirectToAction("InternalError", "Error", new { errorMessage = resultSub.Message });
           return View();
        }
    }
}
