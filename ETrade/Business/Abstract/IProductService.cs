using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface IProductService
    {
        IResult Add(ProductDetailDto productDetailDto);
        IResult Update(ProductDetailDto productDetailDto);
        IResult UpdatePrice(int productId, decimal newPrice);
        IDataResult<List<ProductDetailDto>> GetAll();
        IDataResult<List<ProductDetailDto>> GetByCategoryId(int categoryId);
        IDataResult<ProductDetailDto> GetById(int productId);
        IDataResult<List<ProductDetailDto>> GetByBrandId(int brandId);
    }
}
