using BaByBoi.DataAccess.Helper;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.DataAccess.Service.VNpayService;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Common.Enum;
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
        private readonly IVnpayService _vnpayService;

        public PaymentOrderModel(IProductService productService, IOrderService orderService, IPaymentService paymentService, IVnpayService vnpayService)
        {
            _productService = productService;
            _orderService = orderService;
            _paymentService = paymentService;
            _vnpayService = vnpayService;
        }

        public List<Payment> paymentMethod { get; set; }
        public List<OrderDetail> orderDetailsPurchase { get; set; }
        public Order order { get; set; } = new Order{
            OrderCode = Guid.NewGuid().ToString()          
        };
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

        public async Task<IActionResult> OnPostVnpayAction(int paymentID, double totalPrice)
        {
            getShoppingCartFromSession();
            var paymentMethod = await _paymentService.getPayment(paymentID);
            if (paymentMethod.PaymentName!.ToLower().Equals(PayMethod.VNPay.ToLower()))
            {
                var vnpayModel = new VNPaymentRequestModel
                {
                    TotalPrice = totalPrice,
                    CreatedDate = DateTime.Now,
                    Descriptuon = $"Thanh toán đơn hàng tại BabiBoi Store lúc {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}",
                    OrderCode = order.OrderCode!,
                };
                return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, vnpayModel,paymentID, totalPrice));
            }
            return RedirectToPage(new { handler = "PlaceOrder", paymentID, totalPrice, orderCode = order.OrderCode });
        }

        public async Task<IActionResult> OnGetPlaceOrder(int paymentID, double totalPrice, string orderCode)
        {
            getShoppingCartFromSession();
            if (orderDetailsPurchase == null || orderDetailsPurchase.Count() == 0)
            {
                return RedirectToPage("ShoppingCart");

            }
            getCustomerFromSession();
            if (Customer != null)
            {
                return RedirectToPage("Login");
            }

            var payResponse = _vnpayService.PaymentExecute(Request.Query);

            //if (payResponse == null || payResponse.VnPayResponseCode != "00")
            //{
            //    TempData["ErrorMessage"] = $"Lỗi thanh toán VN Pay: {payResponse!.VnPayResponseCode}";
            //    return RedirectToPage("ShoppingCart");
            //}
            //if (HttpContext.Request.Query.ContainsKey("paymentID"))
            //{
            //    paymentID = int.Parse(HttpContext.Request.Query["paymentID"]!);
            //}
            //if (HttpContext.Request.Query.ContainsKey("totalPrice"))
            //{
            //    totalPrice = double.Parse(HttpContext.Request.Query["totalPrice"]!);
            //}

            order.OrderCode = orderCode;
            order.PaymentId = paymentID;
            order.TotalProduct = orderDetailsPurchase.Count();
            order.OrderDetails = orderDetailsPurchase;
            order.TotalPrice = totalPrice;

            
            orderDetailsPurchase.ForEach(od =>
            {
                od.Order = null!;
                od.Product = null!;
                od.ProductSize = null!;
            });

           
            var result = await _orderService.Insert(order);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Đã đặt hàng thành công";
                return RedirectToPage("OrderHistory");
            }
            return this.Page();
        }
    }
}
