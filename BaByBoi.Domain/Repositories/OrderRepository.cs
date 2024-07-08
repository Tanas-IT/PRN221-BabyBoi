using BaByBoi.Domain.BusinessModel;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        public async Task<List<LineChartModels>> GetAllOrderByMonth()
        {
            var order = await _context.Orders.ToListAsync();
            var orderTotalsByMonth = order
             .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month })
             .Select(g => new LineChartModels()
             {
                 Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month),
                 Year = g.Key.Year,
                 TotalRevenue = g.Sum(o => o.TotalPrice)
             })
             .OrderBy(x => Array.IndexOf(new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" }, x.Month)).ToList();
            return orderTotalsByMonth;
        }
        
        public async Task<List<Order>> GetAllOrder()
        {
            var result = await _context.Orders
                                .Include(x => x.User)
                                .Include(x => x.Payment)
                                .Include(x => x.Voucher)
                                .ToListAsync();
            return result;
        }
    }
}
