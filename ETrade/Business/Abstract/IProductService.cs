using System;
using System.Collections.Generic;
using System.Data;
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
        IResult Update(Product product);
        IResult UpdatePrice(int productId, decimal newPrice);
        IDataResult<List<ProductDetailDto>> GetAll();
        IDataResult<List<ProductDetailDto>> GetByCategoryId(int categoryId);
        IDataResult<ProductDetailDto> GetById(int productId);
        IDataResult<List<ProductDetailDto>> GetByBrandId(int brandId);
        IDataResult<int> GetStockInformation(int productId);
        IResult Sell(int productId, int saleAmount);
        IDataResult<List<ProductDetailDto>> Search(string query);
        IDataResult<List<Product>> GetByStock(int stock);
        IResult Buy(int productId, int unit);
        IDataResult<List<Product>> Index();
    }
}
