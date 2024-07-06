using BaByBoi.DataAccess.Common.Enum;
using BaByBoi.DataAccess.Service.Interface;
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
            entity.OrderCode = Guid.NewGuid().ToString();
            entity.OrderDate = DateTime.Now;
            entity.Status = (int)OrderStatus.WaitingAccept;
            entity.Payment = null;
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
    }
}
