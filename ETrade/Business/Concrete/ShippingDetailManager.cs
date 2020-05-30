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
        //delete fonksiyonu ekle
        public IDataResult<List<ShippingDetail>> GetList()
        {
            throw new NotImplementedException();
        }

        public IDataResult<ShippingDetail> GetById(int shippingDetailId)
        {
            throw new NotImplementedException();
        }
    }
}
