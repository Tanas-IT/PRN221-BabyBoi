using BaByBoi.DataAccess.Common.Enum;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.BusinessModel;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service
{
    public class OrderSerivce : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderSerivce(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<bool> deleteById(string orderCode)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Order>> getAllOrderOfUser(int UserID)
         => await _unitOfWork.OrderRepository.GetAllOrderOfCustomer(UserID);

        public async Task<Order> GetByOrderCode(string code)
        => await _unitOfWork.OrderRepository.GetByOrderCode(code);

        public async Task<Order> Insert(Order entity)
        {
            var result = false;
            entity.OrderCode = entity.OrderCode;
            entity.OrderDate = DateTime.Now;
            entity.Status = (int)OrderStatus.WaitingAccept;
            entity.Payment = null;
            foreach (var item in entity.OrderDetails)
            {
                var productSize = await _unitOfWork.ProductRepository.GetProductsSizesBySpecificSizeAsync(item.ProductId, item.SizeId);
                productSize.Quantity = productSize.Quantity - item.Quantity;
                await _unitOfWork.ProductSizeRepository.Update(productSize!);
            }

            result = await _unitOfWork.OrderRepository.AddAsync(entity);

            if (result)
            {
                return entity;
            }
            return null! ;
        }

        public async Task<Order> update(Order entityUpdate)
        {
            var result = await _unitOfWork.OrderRepository.UpdateAsync(entityUpdate);
            if (result)
            {
                return entityUpdate;
            }
            return null!;
        }

        public async Task<List<LineChartModels>> GetAllOrderByMonth()
        {
            var result = await _unitOfWork.OrderRepository.GetAllOrderByMonth();
            if(result != null)
            {
                return result;
            }
            return null;
        }
        public async Task<List<Order>> GetAllOrder()
        {
            var result = await _unitOfWork.OrderRepository.GetAllOrder();
            return result.ToList();
        }
        public async Task<List<OrderDetail>> GetOrderDetailById(int orderId)
        {
            var result = await _unitOfWork.OrderRepository.GetOrderDetailsById(orderId);
            return result.ToList();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var result = await _unitOfWork.OrderRepository.GetOrderById(orderId);
            return result;
        }

        public async Task<bool> AddFeedback(Order OrderFeedback)
        {
            var result = await _unitOfWork.OrderRepository.AddFeedback(OrderFeedback);
            return result;
        }

    }
}
