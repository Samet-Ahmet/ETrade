using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrete;
using Entities.Dtos;

namespace WebUI.Models
{
    public class ProductDetailViewModel
    {
        public ProductDetailDto ProductDetailDto { get; set; }
        public List<CommentDto> Comments { get; set; }
        public Comment Comment { get; set; }
    }
}
