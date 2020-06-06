using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IShippingDetailService
    {
        IResult Add(ShippingDetail shippingDetail);
        IResult Delete(ShippingDetail shippingDetail);
        IDataResult<List<ShippingDetail>> GetList(int userId);
        IDataResult<ShippingDetail> GetById(int shippingDetailId);
        IResult Update(ShippingDetail shippingDetail);

    }
}
