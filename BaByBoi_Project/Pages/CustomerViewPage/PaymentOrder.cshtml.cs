using BaByBoi.DataAccess.Helper;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.DataAccess.Service.VNpayService;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Common.Enum;
using BaByBoi_Project.Extensions;
using FUMiniHotelManagement.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace BaByBoi_Project.Pages.CustomerViewPage
{
    public class PaymentOrderModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IVnpayService _vnpayService;
        private readonly IVoucherService _voucherService;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public PaymentOrderModel(IProductService productService, IOrderService orderService, IPaymentService paymentService, IVnpayService vnpayService, IVoucherService voucherService, IHubContext<SignalrServer> signalRHub)
        {
            _productService = productService;
            _orderService = orderService;
            _paymentService = paymentService;
            _vnpayService = vnpayService;
            _voucherService = voucherService;
            _signalRHub = signalRHub;
        }
        public List<Payment> paymentMethod { get; set; }
        public List<OrderDetail> orderDetailsPurchase { get; set; }
        public Order order { get; set; } = new Order
        {
            OrderCode = Guid.NewGuid().ToString()
        };

        public double totalPay { get; set; }

        public User Customer { get; set; }
        public Voucher voucher { get; set; }
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
            Customer = HttpContext.Session.GetObjectFromJson<User>("User");
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
            foreach (var item in orderDetailsPurchase)
            {
                totalPay += (double)(Math.Ceiling((double)(item.ProductSize.Price - (item.ProductSize.Price * item.Product.Discount) / 100)!) * item.Quantity)!;
            }
            await getPaymnetMethod();

            // Lấy mã voucher từ session
            var appliedVoucherCode = HttpContext.Session.GetString("AppliedVoucher");
            if (!string.IsNullOrEmpty(appliedVoucherCode))
            {
                voucher = await _voucherService.getVoucherByCode(appliedVoucherCode);
                if (voucher != null && DateTime.Now >= voucher.StartDate && DateTime.Now <= voucher.EndDate && voucher.Status == (int)StatusExist.Exist)
                {
                    var discount = totalPay * voucher.Percent!.Value / 100;
                    if (discount > voucher.TriggerValue)
                    {
                        discount = voucher.TriggerValue.Value;
                    }
                    totalPay = totalPay - discount;
                }
            }
        }

        public async Task<IActionResult> OnPostVnpayAction(int paymentId, double totalPrice, int voucherId)
        {
            getShoppingCartFromSession();
            var paymentMethod = await _paymentService.getPayment(paymentId);
            if (paymentMethod.PaymentName!.ToLower().Equals(PayMethod.VNPay.ToLower()))
            {
                var vnpayModel = new VNPaymentRequestModel
                {
                    TotalPrice = totalPrice,
                    CreatedDate = DateTime.Now,
                    Descriptuon = $"Thanh toán đơn hàng tại BabiBoi Store lúc {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}",
                    OrderCode = order.OrderCode!,
                };
                return Redirect(_vnpayService.CreatePaymentUrl(HttpContext, vnpayModel, paymentId, totalPrice, voucherId));
            }
            return RedirectToPage(new { handler = "PlaceOrder", paymentId, totalPrice, orderCode = order.OrderCode, voucherId = voucherId });
        }

        public async Task<IActionResult> OnPostApplyVoucher(string voucherCode)
        {
            LoadMessagesFromTempData();
            getShoppingCartFromSession();
            getCustomerFromSession();
            await getPaymnetMethod();
            voucher = await _voucherService.getVoucherByCode(voucherCode);
            foreach (var item in orderDetailsPurchase)
            {
                totalPay += (double)(Math.Ceiling((double)(item.ProductSize.Price - (item.ProductSize.Price * item.Product.Discount) / 100)!) * item.Quantity)!;
            }

            if (voucher == null || DateTime.Now < voucher.StartDate || DateTime.Now > voucher.EndDate || voucher.Status != (int)StatusExist.Exist)
            {
                ViewData["ErrorMessage"] = "Voucher đã hết hạn";
            }
            else
            {
                var discount = totalPay * (voucher.Percent!.Value) / 100;
                if (discount > voucher.TriggerValue)
                {
                    discount = voucher.TriggerValue.Value;
                }
                totalPay = totalPay - discount;

                // Lưu voucher vào session
                HttpContext.Session.SetString("AppliedVoucher", voucherCode);
            }
            return this.Page();
        }

        public async Task<IActionResult> OnGetPlaceOrder(int paymentID, double totalPrice, string orderCode, int voucherId)
        {
            getShoppingCartFromSession();
            if (orderDetailsPurchase == null || orderDetailsPurchase.Count() == 0)
            {
                return RedirectToPage("ShoppingCart");

            }
            getCustomerFromSession();
            if (Customer == null)
            {
                return RedirectToPage("/Guest/LoginPage/Index");
            }

            var payResponse = _vnpayService.PaymentExecute(Request.Query);

            order.OrderCode = orderCode;
            order.PaymentId = paymentID;
            order.TotalProduct = orderDetailsPurchase.Count();
            order.OrderDetails = orderDetailsPurchase;
            order.TotalPrice = totalPrice;
            order.UserId = Customer.UserId;
            if (voucherId != 0)
            {
                order.VoucherId = voucherId;
            }

            orderDetailsPurchase.ForEach(od =>
            {
                od.Order = null!;
                od.Product = null!;
                od.ProductSize = null!;
            });

            var result = await _orderService.Insert(order);
            HttpContext.Session.Remove("ListPurchase");
            HttpContext.Session.Remove("OrderList");
            if (result != null)
            {
                TempData["SuccessMessage"] = "Đã đặt hàng thành công";

                int newOrderCount = await _orderService.GetNewOrderCountAsync();
                int allOrderCount = await _orderService.GetAllOrderCountAsync();

                await _signalRHub.Clients.All.SendAsync("ReceiveNewOrderCount", newOrderCount);
                await _signalRHub.Clients.All.SendAsync("ReceiveAllOrderCount", allOrderCount);
                await _signalRHub.Clients.All.SendAsync("ReceiveNewOrder");

                return RedirectToPage("/HistoryOrder/Detail", new { orderCode = order.OrderCode });
            }
            return this.Page();
        }
    }
}
