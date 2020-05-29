using System;
using System.Collections.Generic;
using System.Text;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class ProductDetailDto
    {
        public Product Product { get; set; }
        public List<ProductPhotoPath> ProductPhotoPaths { get; set; }
    }
}
