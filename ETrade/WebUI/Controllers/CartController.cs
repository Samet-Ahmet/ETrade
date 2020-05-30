using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
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

        public CartController(ICartService cartService, ICartSessionHelper cartSessionHelper, IProductService productService)
        {
            _cartService = cartService;
            _cartSessionHelper = cartSessionHelper;
            _productService = productService;
        }

        public IActionResult AddToCart(int productId)
        {
            //ürünü çek
            var dataResult = _productService.GetById(productId);

            ProductDetailDto productDetailDto = dataResult.Data;

            var cart = _cartSessionHelper.GetCart("cart");

            _cartService.AddToCart(cart, productDetailDto.Product);

            _cartSessionHelper.SetCart("cart", cart);

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

        [HttpGet]
        public IActionResult Complete()
        {
            var model = new ShippingDetailsViewModel
            {
                ShippingDetails = 
            };
            return View(model);
        }

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
    }
}
