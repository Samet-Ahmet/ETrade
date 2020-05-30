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
        IDataResult<List<ShippingDetail>> GetList();
        IDataResult<ShippingDetail> GetById(int shippingDetailId);
    }
}
