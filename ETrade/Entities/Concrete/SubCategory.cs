using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Abstract;

namespace Entities.Concrete
{
    public class SubCategory : IEntity
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
