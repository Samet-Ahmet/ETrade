using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models
{
    public class RegisterViewModel
    {
        public UserForRegisterDto UserForRegisterDto { get; set; }
        public List<SelectListItem> GenderNamesSelectItems { get; set; }
   
    }
}
