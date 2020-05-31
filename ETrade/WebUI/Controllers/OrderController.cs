using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using DataAccess.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "Customer")]
    public class OrderController : Controller
    {
        private IOrderService _orderService;
        private IUserService _userService;
        

        public OrderController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetByEmail(string email)
        {
            var model = new OrderListViewModel();

            var result = _userService.GetByMail(email);

            if (!result.Success)
            {
                return RedirectToAction("InternalError", "Error", new { errorMessage = result.Message });
            }

            var result2 =_orderService.GetOrders(result.Data.Id);

            if (!result2.Success)
            {
                TempData.Add(TempDataTypes.ThereIsNoOrder,Messages.ThereIsNoOrder);
                return View(model);
            }

            model.Orders = result2.Data;

            return View(model);

        }
    }
}
