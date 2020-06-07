using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;
using Entities.Concrete;

namespace Entities.Dtos
{
    public class CategoryDto : IDto
    {
        public Category Category { get; set; }
        public int MainCategoryId { get; set; }

    }
}
