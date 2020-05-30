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
    public class ShippingDetailManager : IShippingDetailService
    {
        private IShippingDetailDal _shippingDetailDal;

        public ShippingDetailManager(IShippingDetailDal shippingDetailDal)
        {
            _shippingDetailDal = shippingDetailDal;
        }

        public IResult Add(ShippingDetail shippingDetail)
        {
            try
            {
                _shippingDetailDal.Add(shippingDetail);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileGettingEntity);
            }
        }

        public IResult Delete(ShippingDetail shippingDetail)
        {
            try
            {
                _shippingDetailDal.Delete(shippingDetail);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileDeletingEntity);
            }
        }

        public IDataResult<List<ShippingDetail>> GetList(int userId)
        {
            try
            {
                return new SuccessDataResult<List<ShippingDetail>>(
                    _shippingDetailDal.GetList(sd => sd.CustomerId == userId));
            }
            catch (Exception)
            {
               return new ErrorDataResult<List<ShippingDetail>>(Messages.ThereIsntShippingDetails);
            }
        }

        public IDataResult<ShippingDetail> GetById(int shippingDetailId)
        {
            try
            {
                return new SuccessDataResult<ShippingDetail>(_shippingDetailDal.Get(sd=>sd.ShippingDetailId == shippingDetailId));
            }
            catch (Exception)
            {
                return new ErrorDataResult<ShippingDetail>(Messages.ErrorWhileGettingEntity);
            }
        }
    }
}
