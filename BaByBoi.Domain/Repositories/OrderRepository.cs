using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly BaByBoiContext _context;

        public OrderRepository(BaByBoiContext context) : base(context)         {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrderOfCustomer(int UserId)
        {
            var OrderList = new List<Order>();
            if(UserId > 0)
            {
                OrderList = await _context.Orders
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.Payment)
                    .Include(x => x.Voucher)
                    .Where(x => x.UserId == UserId).ToListAsync();
            }
            return OrderList;
        }
        public async Task<Order> GetByOrderCode(string OrderCode)
        {
            var order = new Order();
            if(!string.IsNullOrEmpty(OrderCode))
            {

                order = await _context.Orders
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.Payment)
                    .Include(x => x.Voucher)
                    .Where(x => x.OrderCode!.ToUpper().Equals(OrderCode.ToUpper())).FirstOrDefaultAsync();
            }
            return order!;
        }
    }
}
