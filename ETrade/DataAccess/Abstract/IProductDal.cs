using System;
using System.Collections.Generic;
using System.Text;
using Core.DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        int Add(Product product);
    }
}
