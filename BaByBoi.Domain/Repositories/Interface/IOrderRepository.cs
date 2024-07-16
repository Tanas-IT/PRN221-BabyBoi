using BaByBoi.Domain.BusinessModel;
using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories.Interface
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order> GetByOrderCode(string OrderCode);
        Task<List<Order>> GetAllOrderOfCustomer(int UserId);
        Task<List<LineChartModels>> GetAllOrderByMonth();
        Task<List<Order>> GetAllOrder();
        Task<List<OrderDetail>> GetOrderDetailsById(int orderId);
        Task<Order> GetOrderById(int orderId);
        Task<bool> AddFeedback(Order OrderFeedback);

    }
}
