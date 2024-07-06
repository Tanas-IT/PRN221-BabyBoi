using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service.Interface
{
    public interface IOrderService
    {
        Task<Order> GetByOrderCode(string code);
        Task<List<Order>> getAllOrderOfUser(int UserID);
        Task<Order> Insert(Order entity);
        Task<Order> update(Order entityUpdate);
        Task<bool> deleteById(string orderCode);
    }
}
