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

    }
}
