using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index(int categoryId = -1)
        {
            if (categoryId == -1)
            {
                var result = _productService.GetAll();
                if (!result.Success)
                {
                    return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
                }

                var model = new ProductListViewModel
                {
                    ProductDetailDtos = result.Data
                };
                return View(model);
            }

            var result2 = _productService.GetByCategoryId(categoryId);
            if (!result2.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result2.Message });
            }

            var model2 = new ProductListViewModel
            {
                ProductDetailDtos = result2.Data
            };
            return View(model2);
        }
    }
}
