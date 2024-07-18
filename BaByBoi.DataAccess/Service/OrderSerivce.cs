using BaByBoi.DataAccess.Common.Enum;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.DataAccess.Service.WorkerService;
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
        private readonly IEmailService _emailService;

        public OrderSerivce(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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
            return null!;
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
            if (result != null)
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

        public async Task<int> GetNewOrderCountAsync()
                => await _unitOfWork.OrderRepository.GetNewOrderCountAsync();

        public async Task<List<Order>> GetOrderByStatus(int status)
                => await _unitOfWork.OrderRepository.GetOrderByStatus(status);

        public async Task<bool> UpdateOrderStatus(int id, int status)
                => await _unitOfWork.OrderRepository.UpdateOrderStatus(id, status);

        public async Task<double> GetTotalRevenueAsync()
                => await _unitOfWork.OrderRepository.GetTotalRevenueAsync();

        public async Task CheckOrderStatusexpired()
        {
            var ordersExpired = await _unitOfWork.OrderRepository.CheckOrderStatusexpired();
            if (ordersExpired != null)
            {
                foreach (var item in ordersExpired)
                {
                    if (item.Status == (int)OrderStatus.IsExpired)
                    {

                        var user = await _unitOfWork.UserRepository.GetById(item.UserId.Value);
                        var order = await _unitOfWork.OrderRepository.GetOrderById(item.OrderId);
                        var subject = "Đơn hàng của bạn đã bị huỷ do 2 ngày chưa được duyệt";
                        var emailBody = BuildOrderEmailBody(order, user);
                        await _emailService.SendEmailAsync(user.Email, subject, emailBody);

                    }
                }
            }
        }

        private string BuildOrderEmailBody(Order order, User user)
        {
            var sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<style>");
            sb.Append("body { font-family: Arial, sans-serif; line-height: 1.6; }");
            sb.Append("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
            sb.Append("th, td { padding: 8px; border: 1px solid #dddddd; text-align: center; }");
            sb.Append("th { background-color: #21409A; color: white; }");
            sb.Append("</style>");
            sb.Append("</head>");
            sb.Append("<body>");

            sb.Append($"<p>Xin Chào {user.FullName},</p>");

            sb.Append($"<p>Đơn hàng của bạn (Mã: {order.OrderCode}) đã bị huỷ do hết hạn.</p>");
            sb.Append("<p>Vui lòng liên hệ với chúng tôi để được hỗ trợ thêm.</p>");
            sb.Append("<p>Chúng tôi xin lỗi vì bất kỳ sự bất tiện nào có thể gây ra.</p>");

            sb.Append("<table>");
            sb.Append("<tr><th>Mã sản phẩm</th><th>Tên sản phẩm</th><th>Kích thước</th><th>Số lượng</th><th>Giá</th></tr>");
            foreach (var detail in order.OrderDetails)
            {
                sb.Append($"<tr><td>{detail.ProductId}</td><td>{detail.Product?.ProductName}</td><td>{detail.ProductSize?.Size?.Description}</td><td>{detail.Quantity}</td><td>{detail.Price}</td></tr>");
            }
            sb.Append("</table>");

            sb.Append($"<p><strong>Tổng giá: {order.TotalPrice}</strong></p>");
            sb.Append("<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>");
            sb.Append("<p>Trân trọng,<br/>");
            sb.Append("BaByBoi</p>");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
