using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DomainModels;

namespace Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDetailDal _orderDetailDal;
        private IOrderDal _orderDal;
        private IProductService _productService;

        public OrderManager(IOrderDal orderDal, IOrderDetailDal orderDetailDal, IProductService productService)
        {
            _orderDal = orderDal;
            _orderDetailDal = orderDetailDal;
            _productService = productService;
        }

        public IResult Add(ShippingDetail shippingDetail, Cart cart)
        {
            decimal totalPrice = 0;
            var now = DateTime.Now;
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var cartLine in cart.CartLines)
            {
                totalPrice += cartLine.Quantity * cartLine.Product.UnitPrice;
                orderDetails.Add(new OrderDetail
                {
                    ProductId = cartLine.Product.ProductId,
                    UnitPrice = cartLine.Product.UnitPrice,
                    Quantity = (short)cartLine.Quantity
                });
            }
            Random r = new Random();
            var trackingNumber = r.Next(111111, 1999999999);
            var order = new Order
            {
                ShippingDetailId = shippingDetail.ShippingDetailId,
                CustomerId = shippingDetail.CustomerId,
                Delivered = false,
                OrderDate = now,
                Price = totalPrice,
                ShippedDate = now,
                TrackingNumber = trackingNumber
            };

            try
            {
                _orderDal.Add(order);
                Thread.Sleep(100);
                var orderId = _orderDal.Get(o => o.CustomerId == shippingDetail.CustomerId 
                                                 && o.ShippingDetailId == shippingDetail.ShippingDetailId
                                                 && o.Price == totalPrice
                                                 && o.TrackingNumber == trackingNumber).OrderId;
                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderId = orderId;
                    _orderDetailDal.Add(orderDetail);
                }

                foreach (var cartLine in cart.CartLines)
                {
                    _productService.Sell(cartLine.Product.ProductId, cartLine.Quantity);

                }

                return new SuccessResult();

            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileAddingEntity);
            }
        }

        public IDataResult<List<Order>> GetOrders(int customerId)
        {
            try
            {
                return new SuccessDataResult<List<Order>>(_orderDal.GetList(o => o.CustomerId == customerId));

            }
            catch (Exception)
            {
                return new ErrorDataResult<List<Order>>(Messages.ErrorWhileGettingEntity);
            }

        }

        public IDataResult<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            try
            {
                return new SuccessDataResult<List<OrderDetail>>(_orderDetailDal.GetList(od => od.OrderId == orderId));

            }
            catch (Exception)
            {
                return new ErrorDataResult<List<OrderDetail>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IResult Delete(int orderId)
        {
            throw new NotImplementedException();
            
        }

        public IDataResult<List<Order>> GetAll(bool ship = false) //true ise teslim edilmeyenleri getir 
        {
            try
            {
                if (!ship)
                {
                    return new SuccessDataResult<List<Order>>(_orderDal.GetList());
                }
                return new SuccessDataResult<List<Order>>(_orderDal.GetList(o=>o.Delivered == false));
            }
            catch (Exception)
            {
                return new ErrorDataResult<List<Order>>(Messages.ErrorWhileGettingEntity);
            }
        }

        public IResult Deliver(int orderId)
        {
            try
            {
                var order = _orderDal.Get(o => o.OrderId == orderId);
                order.Delivered = true;
                _orderDal.Update(order);
                return new SuccessResult();
            }
            catch (Exception)
            {
                return new ErrorResult(Messages.ErrorWhileUpdatingEntity);
            }
        }
    }
}
