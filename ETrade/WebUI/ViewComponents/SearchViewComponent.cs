using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebUI.Models;

namespace WebUI.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {
            var model = new SearchViewModel
            {
                Query = new string("")
            };
            return View(model);
        }
    }
}
