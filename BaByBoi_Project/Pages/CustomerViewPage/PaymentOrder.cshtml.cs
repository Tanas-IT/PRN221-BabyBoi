using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.CustomerViewPage
{
    public class PaymentOrderModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        public PaymentOrderModel(IProductService productService, IOrderService orderService, IPaymentService paymentService)
        {
            _productService = productService;
            _orderService = orderService;
            _paymentService = paymentService;
        }

        public List<Payment> paymentMethod { get; set; }
        public List<OrderDetail> orderDetailsPurchase { get; set; }
        public Order order { get; set; } = new Order();
        public User Customer { get; set; }

        private void getShoppingCartFromSession()
        {
            orderDetailsPurchase = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("ListPurchase") ?? new List<OrderDetail>();
        }

        private async Task getPaymnetMethod()
        {
            paymentMethod = await _paymentService.getAllPaymentMethod();
        }
        private void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        private void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }
        private void getCustomerFromSession()
        {
            Customer = HttpContext.Session.GetObjectFromJson<User>("Customer");
        }
        private void LoadMessagesFromTempData()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
        }
        public async Task OnGet()
        {
            LoadMessagesFromTempData();
            getShoppingCartFromSession();
            getCustomerFromSession();
            await getPaymnetMethod();
        }

        public async Task<IActionResult> OnPostPlaceOrder(int paymentID, double totalPrice)
        {
            getShoppingCartFromSession();
            getCustomerFromSession();

            orderDetailsPurchase.ForEach(od =>
            {
                od.Order = null!;
                od.Product = null!;
                od.ProductSize = null!;
            });

            order.PaymentId = paymentID;
            order.TotalProduct = orderDetailsPurchase.Count();
            order.OrderDetails = orderDetailsPurchase;
            order.TotalPrice = totalPrice;
            var result = await _orderService.Insert(order);
            if (result != null)
            {
                return RedirectToPage("OrderHistory");
            }
            return this.Page();
        }
    }
}
