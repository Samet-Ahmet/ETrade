using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models
{
    public class ManageProfileViewModel
    {
        public User User { get; set; }
        public List<SelectListItem> GenderNamesSelectItems { get; set; }
    }
}
