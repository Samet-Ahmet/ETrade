using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductService _productService;
        private ICommentService _commentService;
        private IUserService _userService;

        public ProductController(IProductService productService, ICommentService commentService, IUserService userService)
        {
            _productService = productService;
            _commentService = commentService;
            _userService = userService;
        }

        public IActionResult Index(int categoryId = -1, string query = null)
        {
            if (query == null)
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
            else //search
            {
                var result = _productService.Search(query);
                var model = new ProductListViewModel
                {
                    ProductDetailDtos = result.Data
                };
                return View(model);
            }

        }

        public IActionResult Detail(int productId)
        {
            var result = _productService.GetById(productId);
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            var comments = _commentService.GetByProductId(result.Data.Product.ProductId);
            if (!comments.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = comments.Message });
            }
            var model = new ProductDetailViewModel
            {
                ProductDetailDto = result.Data,
                Comments = comments.Data,
                Comment = new Comment()
            };
            
            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            var email = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value;
            comment.UserId = _userService.GetByMail(email).Data.Id;
            var result = _commentService.Add(comment);
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }
            return RedirectToAction("Detail", "Product", new {productId = comment.ProductId});
        }
    }
}
