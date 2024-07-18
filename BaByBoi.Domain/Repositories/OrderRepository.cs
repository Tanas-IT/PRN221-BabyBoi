using BaByBoi.DataAccess.Common.Enum;
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

        public OrderRepository(BaByBoiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllOrderOfCustomer(int UserId)
        {
            var OrderList = new List<Order>();
            if (UserId > 0)
            {
                OrderList = await _context.Orders
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.Payment)
                    .Include(x => x.Voucher)
                    .Where(x => x.UserId == UserId)
                    .OrderBy(x => x.Status)
                    .ThenByDescending(x => x.OrderId)
                    .ToListAsync();
            }
            return OrderList;
        }
        public async Task<Order> GetByOrderCode(string OrderCode)
        {
            var order = new Order();
            if (!string.IsNullOrEmpty(OrderCode))
            {

                order = await _context.Orders
                    .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                    .Include(x => x.User)
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
        public async Task<List<OrderDetail>> GetOrderDetailsById(int orderId)
        {
            var result = await _context.OrderDetails
                                    .Where(x => x.OrderId == orderId)
                                    .Include(x => x.Product)
                                    .Include(x => x.Product.ProductImages)
                                    .Include(x => x.Product.ProductSizes)
                                    .ThenInclude(x => x.Size)
                                    .Include(x => x.Order.User)
                                    .ToListAsync();
            return result;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var result = await _context.Orders
                            .Include(x => x.OrderDetails)
                                .ThenInclude(x => x.Product)
                                .ThenInclude(x => x.ProductSizes)
                                .ThenInclude(x => x.Size)
                            .Include(x => x.Voucher)
                            .Include(x => x.Payment)
                            .Include(x => x.User)
                            .FirstOrDefaultAsync(x => x.OrderId == orderId);
            return result;
        }

        public async Task<bool> AddFeedback(Order OrderFeedback)
        {
            var GetOrderFeeback = await _context.Orders.FirstOrDefaultAsync(x => x.OrderCode == OrderFeedback.OrderCode);
            if (GetOrderFeeback != null)
            {
                GetOrderFeeback.Rating = OrderFeedback.Rating;
                GetOrderFeeback.Feedback = OrderFeedback.Feedback;
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<int> GetNewOrderCountAsync()
        {
            var count = await _context.Orders
                .Where(o => o.Status == (int)OrderStatus.WaitingAccept)
                .CountAsync();
            return count;
        }

        public async Task<List<Order>> GetOrderByStatus(int status)
        {
            var query = _context.Orders
                           .Include(o => o.Voucher)
                           .Include(o => o.Payment)
                           .Include(o => o.User)
                           .AsQueryable();

            if (status != 0)
            {
                query = query.Where(o => o.Status == status);
            }

            var orders = await query.OrderByDescending(o => o.OrderId).ToListAsync();
            return orders;
        }

        public async Task<bool> UpdateOrderStatus(int id, int status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }
            order.Status = status;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<double> GetTotalRevenueAsync()
        {
            var totalRevenue = await _context.Orders
            .Where(o => o.Status == (int)OrderStatus.IsConfirmed)
            .SumAsync(o => o.TotalPrice ?? 0.0);

            return totalRevenue;
        }

        public async Task<int> GetAllOrderCountAsync()
        {
            var count = await _context.Orders.CountAsync();
            return count;
        }
    }
}
