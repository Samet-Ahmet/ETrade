using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;

namespace WebUI.Models
{
    public class AddProductViewModel
    {
        public ProductDetailDto ProductDetailDto { get; set; }
    }
}
