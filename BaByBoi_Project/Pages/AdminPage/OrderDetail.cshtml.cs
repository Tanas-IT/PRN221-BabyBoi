using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class OrderDetailModel : PageModel
    {
        private readonly IOrderService _orderService;

        public OrderDetailModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public List<OrderDetail> ListOrderDetail { get; set; }
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public string FeedBackContent { get; set; }

        private async Task LoadData(string orderCode)
        {
            Order = await _orderService.GetByOrderCode(orderCode);
            ListOrderDetail = await _orderService.GetOrderDetailById(Order.OrderId);
        }
        public async Task<IActionResult> OnGet(string orderCode)
        {
            if (orderCode == null)
            {
                return RedirectToPage("Index");
            }
            else
            {
                //int orderIdInt = int.Parse(orderCode);
                await LoadData(orderCode);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAddFeedback(IFormCollection form)
        {
            Order OrderFeedback = new Order();
            string rating = form["rating"];
            string orderCode = form["OrderCode"];
            if (rating != null)
            {
                OrderFeedback.Rating = int.Parse(rating);
            }
            else
            {
                OrderFeedback.Rating = 0;
            }
            OrderFeedback.OrderCode = orderCode;
            OrderFeedback.Feedback = FeedBackContent;
            var result = await _orderService.AddFeedback(OrderFeedback);
            if (result)
            {
                ViewData["FeedbackMessage"] = "Add Feedback success";
            }
            else
            {
                ViewData["FeedbackErrorMessage"] = "Add Feedback failed";
            }
            await LoadData(OrderFeedback.OrderCode!);
            return Page();

        }
    }
}
