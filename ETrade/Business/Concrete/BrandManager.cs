using System;
using System.Collections.Generic;
using System.Text;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private IBrandDal _brandDal;
        private IProductService _productService;

        public BrandManager(IBrandDal brandDal, IProductService productService)
        {
            _brandDal = brandDal;
            _productService = productService;
        }

        public IResult Add(Brand brand)
        {
            try
            {
                _brandDal.Add(brand);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }

        public IResult Update(Brand brand)
        {
            try
            {
                _brandDal.Update(brand);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }

        public IResult Delete(Brand brand)
        {
            try
            {
                int count = _productService.GetByBrandId(brand.BrandId).Data.Count;
                if (count > 0)
                {
                    return new ErrorResult(Messages.BrandCantDeleted);
                }
                _brandDal.Delete(brand);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }

        public IDataResult<List<Brand>> GetAll()
        {
            try
            {
                return new SuccessDataResult<List<Brand>>(_brandDal.GetList());
            }
            catch (Exception )
            {
                return new ErrorDataResult<List<Brand>>(Messages.ErrorWhileGettingEntity);
            }
        }
    }
}
