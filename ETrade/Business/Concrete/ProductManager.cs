using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private IProductPhotoPathDal _productPhotoPathDal;

        public ProductManager(IProductDal productDal, IProductPhotoPathDal productPhotoPathDal)
        {
            _productDal = productDal;
            _productPhotoPathDal = productPhotoPathDal;
        }


        public IResult Add(ProductDetailDto productDetailDto)
        {
            try
            {
                _productDal.Add(productDetailDto.Product);
                Thread.Sleep(200);
                var productId = _productDal.Get(p => p.ProductDef == productDetailDto.Product.ProductDef
                                                     && p.ProductName == productDetailDto.Product.ProductName
                                                     && p.UnitPrice == productDetailDto.Product.UnitPrice
                                                     && p.BrandId == productDetailDto.Product.BrandId
                                                     && p.CategoryId == productDetailDto.Product.CategoryId).ProductId;

                foreach (var productPhotoPath in productDetailDto.ProductPhotoPaths)
                {
                    productPhotoPath.ProductId = productId;
                    _productPhotoPathDal.Add(productPhotoPath);
                }

                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }

        public IResult Update(ProductDetailDto productDetailDto)
        {
            try
            {
                _productDal.Update(productDetailDto.Product);
                foreach (var productPhotoPath in productDetailDto.ProductPhotoPaths)
                {
                    _productPhotoPathDal.Update(productPhotoPath);
                }

                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IResult Update(Product product)
        {
            try
            {
                _productDal.Update(product);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IResult UpdatePrice(int productId, decimal newPrice)
        {
            try
            {
                var product = _productDal.Get(p => p.ProductId == productId);
                product.UnitPrice = newPrice;
                _productDal.Update(product);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IDataResult<List<ProductDetailDto>> GetAll()
        {
            try
            {
                var productDetails = new List<ProductDetailDto>();
                var products = _productDal.GetList();
                foreach (var product in products)
                {
                    productDetails.Add(new ProductDetailDto
                    {
                        Product = product,
                        ProductPhotoPaths = _productPhotoPathDal.GetList(p => p.ProductId == product.ProductId)
                    });
                }

                return new SuccessDataResult<List<ProductDetailDto>>(productDetails);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<List<ProductDetailDto>> GetByCategoryId(int categoryId)
        {
            try
            {
                var productDetails = new List<ProductDetailDto>();
                var products = _productDal.GetList(p => p.CategoryId == categoryId);
                foreach (var product in products)
                {
                    productDetails.Add(new ProductDetailDto
                    {
                        Product = product,
                        ProductPhotoPaths = _productPhotoPathDal.GetList(p => p.ProductId == product.ProductId)
                    });
                }

                return new SuccessDataResult<List<ProductDetailDto>>(productDetails);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<ProductDetailDto> GetById(int productId)
        {
            try
            {
                var product = _productDal.Get(p => p.ProductId == productId);
                var productDetail = new ProductDetailDto
                {
                    Product = product,
                    ProductPhotoPaths = _productPhotoPathDal.GetList(p => p.ProductId == productId)
                };

                return new SuccessDataResult<ProductDetailDto>(productDetail);
            }
            catch (Exception)
            {
                return new ErrorDataResult<ProductDetailDto>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<List<ProductDetailDto>> GetByBrandId(int brandId)
        {
            try
            {
                var productDetails = new List<ProductDetailDto>();
                var products = _productDal.GetList(p => p.BrandId == brandId);
                foreach (var product in products)
                {
                    productDetails.Add(new ProductDetailDto
                    {
                        Product = product,
                        ProductPhotoPaths = _productPhotoPathDal.GetList(p => p.ProductId == product.ProductId)
                    });
                }

                return new SuccessDataResult<List<ProductDetailDto>>(productDetails);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<int> GetStockInformation(int productId)
        {
            try
            {
                var count = _productDal.Get(p => p.ProductId == productId).UnitsInStock;
                return new SuccessDataResult<int>(count);
            }
            catch (Exception)
            {
                return new ErrorDataResult<int>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IResult Sell(int productId, int saleAmount)
        {
            try
            {
                var product = _productDal.Get(p => p.ProductId == productId);
                product.UnitsInStock -= saleAmount;
                _productDal.Update(product);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IDataResult<List<ProductDetailDto>> Search(string query)
        {
            try
            {
                var productDetails = new List<ProductDetailDto>();
                var products = _productDal.GetList(p=>p.ProductName.Contains(query));
                foreach (var product in products)
                {
                    productDetails.Add(new ProductDetailDto
                    {
                        Product = product,
                        ProductPhotoPaths = _productPhotoPathDal.GetList(p => p.ProductId == product.ProductId)
                    });
                }

                return new SuccessDataResult<List<ProductDetailDto>>(productDetails);
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<ProductDetailDto>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IDataResult<List<Product>> GetByStock(int stock)
        {
            try
            {
                if (stock == 0)
                {
                    return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.UnitsInStock <= stock));
                }
                else
                {
                    return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.UnitsInStock <= stock && p.UnitsInStock != 0));
                }
                
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<Product>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IResult Buy(int productId, int unit)
        {
            try
            {
                var product = _productDal.Get(p => p.ProductId == productId);
                product.UnitsInStock += unit;
                _productDal.Update(product);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IDataResult<List<Product>> Index()
        {
            try
            {
                return new SuccessDataResult<List<Product>>(_productDal.GetList());
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<Product>>(Messages.ErrorWhileGettingEntity);
            }
        }
    }
}
