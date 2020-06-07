using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models
{
    public class AddCategoryViewModel
    {
        public CategoryDto CategoryDto { get; set; }
        public List<SelectListItem> categories { get; set; }

    }
}
