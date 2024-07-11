using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.HistoryOrder
{
    public class DetailModel : PageModel
    {
        private readonly IOrderService _orderService;

        public DetailModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public List<OrderDetail> ListOrderDetail { get; set; }
        [BindProperty]
        public Order Order { get; set; }
        [BindProperty]
        public string FeedBackContent { get; set; }

        private async Task LoadData(int OrderId)
        {
            ListOrderDetail = await _orderService.GetOrderDetailById(OrderId);
            Order = await _orderService.GetOrderById(OrderId);
        }
        public async Task<IActionResult> OnGet(string orderId)
        {
                if(orderId == null)
                {
                    return RedirectToPage("Index");
                } else
                {
                    int orderIdInt = int.Parse(orderId);
                    await LoadData(orderIdInt);
                    return Page();
                }
               
        }

        public async Task<IActionResult> OnPostAddFeedback(IFormCollection form) 
        {
            Order OrderFeedback = new Order();
            string rating = form["rating"];
            string orderId = form["OrderId"];
            if(rating != null)
            {
                OrderFeedback.Rating = int.Parse(rating);
            } else
            {
                OrderFeedback.Rating = 0;
            }
            OrderFeedback.OrderId =int.Parse(orderId);
            OrderFeedback.Feedback = FeedBackContent;
            var result = await _orderService.AddFeedback(OrderFeedback);
            if(result)
            {
                ViewData["FeedbackMessage"] = "Add Feedback success";
            } else
            {
                ViewData["FeedbackErrorMessage"] = "Add Feedback failed";
            }
            await LoadData(OrderFeedback.OrderId);
            return Page();
            
        }
    }
}
