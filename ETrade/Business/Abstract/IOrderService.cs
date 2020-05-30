using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DomainModels;

namespace Business.Abstract
{
    public interface IOrderService
    {
        IResult Add(ShippingDetail shippingDetail, Cart cart);
        IDataResult<List<Order>> GetOrders(int customerId);
        IDataResult<List<OrderDetail>> GetOrderDetails(int orderId);
        IResult Delete(int orderId);
    }
}
