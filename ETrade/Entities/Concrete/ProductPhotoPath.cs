using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class ProductPhotoPath : IEntity
    {
        public int ProductId { get; set; }
        public string PhotoPath { get; set; }

    }
}
