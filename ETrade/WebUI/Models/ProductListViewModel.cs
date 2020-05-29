using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Dtos;

namespace WebUI.Models
{
    public class ProductListViewModel
    {
        public List<ProductDetailDto> ProductDetailDtos { get; set; }
    }
}
