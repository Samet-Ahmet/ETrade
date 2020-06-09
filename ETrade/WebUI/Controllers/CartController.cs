using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DomainModels;
using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Helpers;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private ICartSessionHelper _cartSessionHelper;
        private IProductService _productService;
        private IShippingDetailService _shippingDetailService;
        private IUserService _userService;
        private ICityService _cityService;
        private IOrderService _orderService;

        public CartController(ICartService cartService, ICartSessionHelper cartSessionHelper, IProductService productService, IShippingDetailService shippingDetailService, IUserService userService, ICityService cityService, IOrderService orderService)
        {
            _cartService = cartService;
            _cartSessionHelper = cartSessionHelper;
            _productService = productService;
            _shippingDetailService = shippingDetailService;
            _userService = userService;
            _cityService = cityService;
            _orderService = orderService;
        }

        public IActionResult AddToCart(int productId)
        {
            //ürünü çek
            var dataResult = _productService.GetById(productId);

            ProductDetailDto productDetailDto = dataResult.Data;

            var cart = _cartSessionHelper.GetCart("cart");

            _cartService.AddToCart(cart, productDetailDto.Product);

            _cartSessionHelper.SetCart("cart", cart);
            TempData.Remove(TempDataTypes.AddedToCart);
            TempData.Add(TempDataTypes.AddedToCart, productDetailDto.Product.ProductName + " sepete eklendi!");

            return RedirectToAction("Index", "Product");
        }

        public IActionResult RemoveFromCart(int productId, bool removeAll = false)
        {
            //ürünü çek
            var dataResult = _productService.GetById(productId);

            ProductDetailDto productDetailDto = dataResult.Data;
            //sessiondaki sepeti çek
            var cart = _cartSessionHelper.GetCart("cart");

            //productId ye ait satırı bulduk
            var cartLine = cart.CartLines.FirstOrDefault(c => c.Product.ProductId == productId);//linq

            //sayısı birden fazla ise ya da hepsini kaldır ise
            if (cartLine.Quantity == 1 || removeAll)
            {
                //carttan ilgili ürünü sil
                _cartService.RemoveFromCart(cart, productId);
                TempData.Add(TempDataTypes.RemovedFromCart, productDetailDto.Product.ProductName + " sepetten silindi!");
            }
            else
            {
                cartLine.Quantity--;
                TempData.Add(TempDataTypes.DecreasedFromCart, "Sepetten bir adet " + productDetailDto.Product.ProductName + " silindi!");
            }

            //sessiona cartı yeniden ekledik
            _cartSessionHelper.SetCart("cart", cart);

            //alttaki sayfaya yönlendirdik yani aynı sayfaya
            return RedirectToAction("Index", "Cart");

        }

        public IActionResult Index()
        {
            var model = new CartListViewModel
            {
                Cart = _cartSessionHelper.GetCart("cart")
            };
            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult Complete()
        {

            var model = new ShippingDetailsViewModel
            {
                ShippingDetails = new List<ShippingDetail>()
            };
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value).Data;
            var result = _shippingDetailService.GetList(user.Id);
            var cart = _cartSessionHelper.GetCart("cart");
            bool error = false;
            foreach (var cartLine in cart.CartLines.ToList())
            {
                var stock = _productService.GetStockInformation(cartLine.Product.ProductId).Data;

                if (stock == 0) 
                {
                    error = true;
                    _cartService.RemoveFromCart(cart,cartLine.Product.ProductId);
                }

                if (cartLine.Quantity > stock)
                {
                    error = true;
                    cartLine.Quantity = stock;
                }
            }
            _cartSessionHelper.SetCart("cart",cart);
            if (error)
            {
                TempData.Add(TempDataTypes.StockInsufficient, Messages.StockInsufficient);
                return RedirectToAction("Index","Cart");
            }

            if (!result.Success)
            {
                if (result.Message.Equals(Messages.ThereIsntShippingDetails))
                {
                    return RedirectToAction("AddShippingDetail", "Cart");
                }
            }
            
            model.ShippingDetails = result.Data;

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult Complete(ShippingDetail shippingDetail)
        {
            if (!ModelState.IsValid)//dataannotationa göre geçerli mi değil mi
            {
                return View();
            }
            TempData.Add("message", "Siparişiniz başarıyla tamamlandı");
            _cartSessionHelper.Clear();
            return RedirectToAction("Index", "Cart");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult AddShippingDetail()
        {
            var model = new AddShippingDetailViewModel
            {
                ShippingDetail = new ShippingDetail()
            };

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult AddShippingDetail(ShippingDetail shippingDetail)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value).Data;
            shippingDetail.CustomerId = user.Id;
            var result = _shippingDetailService.Add(shippingDetail);

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        public ActionResult GetDistricts(int cityId)
        {
            var model = _cityService.GetDistrictsByCityId(cityId).Data;

            //return Ok(model);
            return Json(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult Complete2(int shippingDetailId)
        {
            var model = new CompleteOrderViewModel
            {
                ShippingDetail = _shippingDetailService.GetById(shippingDetailId).Data,
                Cart = _cartSessionHelper.GetCart("cart")
            };

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult Completed(int shippingDetailId)
        {
            var shippingDetail = _shippingDetailService.GetById(shippingDetailId).Data;
            var result = _orderService.Add(shippingDetail, _cartSessionHelper.GetCart("cart"));

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            _cartSessionHelper.Clear();

            return View();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult EditShippingDetail(int shippingDetailId)
        {
            var result = _shippingDetailService.GetById(shippingDetailId);

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }
            var model = new EditShippingDetailViewModel
            {
                ShippingDetail = result.Data
            };

            return View(model);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public IActionResult EditShippingDetail(ShippingDetail shippingDetail)
        {
            if (!ModelState.IsValid || shippingDetail.DistrictId == 0)
            {
                TempData.Add(TempDataTypes.ShippingDetailInfo,Messages.ShippingDetailInfo);
                return RedirectToAction("EditShippingDetail", "Cart", new { shippingDetailId = shippingDetail.ShippingDetailId });
            }

            var result = _shippingDetailService.Update(shippingDetail);
            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            TempData.Add(TempDataTypes.ManageInfo,Messages.ShippingDetailEditedSuccessfully);
            return RedirectToAction("ListShippingDetails", "Cart");
        }

        [Authorize(Roles = "Customer")]
        [HttpGet]
        public IActionResult ListShippingDetails()
        {

            var model = new ShippingDetailsViewModel
            {
                ShippingDetails = new List<ShippingDetail>()
            };
            var user = _userService.GetByMail(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value).Data;
            var result = _shippingDetailService.GetList(user.Id);

            if (!result.Success)
            {
                if (result.Message.Equals(Messages.ThereIsntShippingDetails))
                {
                    return RedirectToAction("AddShippingDetail", "Cart");
                }
            }

            model.ShippingDetails = result.Data;

            return View(model);
        }


    }
}
