using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Models
{
    public class ProductListViewModel
    {
        public List<ProductDetailDto> ProductDetailDtos { get; set; }
        public string CategoryName { get; set; }
    }
}
