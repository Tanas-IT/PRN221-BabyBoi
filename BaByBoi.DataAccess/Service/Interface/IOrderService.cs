using BaByBoi.Domain.BusinessModel;
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
        Task<List<LineChartModels>> GetAllOrderByMonth();
        Task<List<Order>> GetAllOrder();
        Task<List<OrderDetail>> GetOrderDetailById(int orderId);
        Task<Order> GetOrderById(int orderId);
        Task<bool> AddFeedback(Order OrderFeedback);
    }
}
