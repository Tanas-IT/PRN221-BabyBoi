using BaByBoi.DataAccess.Common.Enum;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi.Domain.Utils;
using BaByBoi_Project.Common.Enum;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class NewOrderModel : PageModel
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public NewOrderModel(IOrderService orderService, IUserService userService, IEmailService emailService)
        {
            _orderService = orderService;
            _userService = userService;
            _emailService = emailService;
        }

        [BindProperty]
        public int CurrentIndex { get; set; }

        [BindProperty]
        public int PageIndex { get; set; } = 1;
        [BindProperty]
        public int PageSize { get; set; } = 5;
        [BindProperty]
        public int TotalPage { get; set; }
        [BindProperty]
        public string Status { get; set; } = "WaitingAccept";
        [BindProperty]

        public List<Order> NewOrderPagin { get; set; }
        public async Task OnGet(string PageIndex, string PageSize)
        {
            await LoadDataAsync(PageIndex, PageSize, (int)(OrderStatus.WaitingAccept));
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
        }

        private async Task LoadDataAsync(string? PageIndex, string PageSize, int status)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("User");
            var result = await _orderService.GetOrderByStatus(status);
            this.PageIndex = 1;
            this.PageSize = 5;
            try
            {
                if (PageIndex == null && PageSize == null)
                {
                    this.PageIndex = PageHelper.DefaultPageIndex;
                    this.PageSize = PageHelper.DefaultPageSize;
                }
                else if (int.Parse(PageIndex) <= 0)
                {
                    this.PageIndex = PageHelper.DefaultPageIndex;
                    this.PageSize = PageHelper.DefaultPageSize;
                }
                else if (int.Parse(PageIndex) > (int)Math.Ceiling(result.Count / (double)int.Parse(PageSize)))
                {
                    this.PageIndex = TotalPage;
                    this.PageSize = int.Parse(PageSize);
                }
                else
                {
                    this.PageIndex = int.Parse(PageIndex);
                    this.PageSize = int.Parse(PageSize);
                }
                NewOrderPagin = PaginatedList<Order>.Create(
                        result.AsQueryable(), this.PageIndex, this.PageSize);
                TotalPage = (int)Math.Ceiling(result.Count / (double)this.PageSize);
                CurrentIndex = this.PageIndex;
            }
            catch (Exception)
            {
                this.PageIndex = PageHelper.DefaultPageIndex;
                this.PageSize = PageHelper.DefaultPageSize;
                NewOrderPagin = PaginatedList<Order>.Create(
                       result.AsQueryable(), this.PageIndex, this.PageSize);
                TotalPage = (int)Math.Ceiling(result.Count / (double)this.PageSize);
                CurrentIndex = this.PageIndex;
            }
        }

        public async Task<IActionResult> OnPostConfirmOrderAsync(int orderId, int customerId, string pageIndex)
        {
            if (HttpContext.Session.GetObjectFromJson<User>("User").RoleId != (int)UserRole.Admin)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Guest/LoginPage");
            }

            if (orderId != 0)
            {
                var result = await _orderService.UpdateOrderStatus(orderId, (int)OrderStatus.IsConfirmed);
                if (result)
                {
                    var user = await _userService.GetByIdAsync(customerId);
                    var order = await _orderService.GetOrderById(orderId);
                    var subject = "Đơn hàng của bạn đã được xác nhận";
                    var emailBody = BuildOrderEmailBody(order, user, true);
                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, emailBody);
                        TempData["SuccessMessage"] = "Đơn hàng đã được xác nhận và email đã được gửi.";
                    }
                    catch (Exception)
                    {
                        TempData["ErrorMessage"] = "Đơn hàng đã được xác nhận, nhưng không thể gửi email.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Không thể xác nhận đơn hàng.";
                }

            }

            await LoadDataAsync(pageIndex, "5", (int)(OrderStatus.WaitingAccept));
            return RedirectToPage(new { pageIndex = PageIndex });
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId, int customerId, string pageIndex)
        {
            if (HttpContext.Session.GetObjectFromJson<User>("User").RoleId != (int)UserRole.Admin)
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Guest/LoginPage");
            }

            if (orderId != 0)
            {
                var result = await _orderService.UpdateOrderStatus(orderId, (int)OrderStatus.IsReject);
                if (result)
                {
                    var user = await _userService.GetByIdAsync(customerId);
                    var order = await _orderService.GetOrderById(orderId);
                    var subject = "Đơn hàng của bạn đã bị từ chối";
                    var emailBody = BuildOrderEmailBody(order, user, false);
                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, emailBody);
                        SetSuccessMessage("Đơn hàng đã bị từ chối và email đã được gửi.");
                    }
                    catch (Exception)
                    {
                        SetErrorMessage("Đơn hàng đã bị từ chối, nhưng không thể gửi email.");
                    }
                }
                else
                {
                    SetErrorMessage("Không thể từ chối đơn hàng.");
                }
            }

            await LoadDataAsync(pageIndex, "5", (int)(OrderStatus.WaitingAccept));
            return RedirectToPage(new { pageIndex = PageIndex });

        }

        private void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        private void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }

        private string BuildOrderEmailBody(Order order, User user, bool isConfirmed)
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

            sb.Append($"<p>Chào {user.FullName},</p>");
            if (isConfirmed)
            {
                sb.Append($"<p>Đơn hàng của bạn (Mã: {order.OrderCode}) đã được xác nhận.</p>");
            }
            else
            {
                sb.Append($"<p>Đơn hàng của bạn (Mã: {order.OrderCode}) đã bị từ chối.</p>");
                sb.Append("<p>Vui lòng liên hệ với chúng tôi để được hỗ trợ thêm.</p>");
                sb.Append("<p>Chúng tôi xin lỗi vì bất kỳ sự bất tiện nào có thể gây ra.</p>");
            }

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
