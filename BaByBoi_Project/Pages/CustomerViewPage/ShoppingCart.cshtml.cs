using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Common;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.CustomerViewPage
{
    public class ShoppingCartModel : PageModel
    {
        private readonly IProductService _productService;

        public ShoppingCartModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public List<string> SelectedProducts { get; set; }


        public List<OrderDetail> OrderList { get; set; } = new List<OrderDetail>();
        public User Customer { get; set; }
        private void getShoppingCartFromSession()
        {
            OrderList = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("OrderList") ?? new List<OrderDetail>();
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

        public void OnGet()
        {
            getCustomerFromSession();
            getShoppingCartFromSession();
            LoadMessagesFromTempData();
        }
        public async void OnPostDeleteInCart(int ProductId)
        {
            getShoppingCartFromSession();
            var product = OrderList.FirstOrDefault(x => x.ProductId == ProductId);
            if (product != null)
            {
                OrderList.Remove(product);
            }
            SetSuccessMessage("Room deleted successfully.");
        }
        private void SaveBookingListToSession()
        {
            HttpContext.Session.SetObjectAsJson("OrderList", OrderList);
        }
        public IActionResult OnPostUpdateInCart(int ProductId, int Quantity, int sizeId)
        {
            getShoppingCartFromSession();
            var product = OrderList.FirstOrDefault(x => x.ProductId == ProductId && x.SizeId == sizeId);
            if (product != null)
            {
                product.Price = Quantity * product.Price;
                product.Quantity = product.Quantity > Quantity ? product.Quantity : Quantity;
                SaveBookingListToSession();
                SetSuccessMessage("Product booking updated successfully.");
            }
            else
            {
                SetErrorMessage("Product is not found.");
            }
            return this.Page();
        }

        public IActionResult OnPostPurchase()
        {
            getShoppingCartFromSession();
            getCustomerFromSession();
            if (SelectedProducts != null && SelectedProducts.Any())
            {
                var purchase = new List<OrderDetail>();
                // Xử lý mua các sản phẩm đã chọn
                foreach (var prosize in SelectedProducts)
                {
                    var eachProduct = ProductSizeHelper.cutID(prosize);
                    var orderDetail = OrderList.Where(x => x.ProductId == eachProduct.ProductId && x.SizeId == eachProduct.SizeId).FirstOrDefault();
                    if (eachProduct != null)
                    {
                        purchase.Add(orderDetail!);
                    }
                }
                HttpContext.Session.SetObjectAsJson("ListPurchase", purchase);
                return RedirectToPage("PaymentOrder"); // chuyển qua trang in ra bill chứ chưa thực sự payment vì còn áp dụng voucher
                // thực hiện logic mua hàng, ví dụ: lưu vào cơ sở dữ liệu, gửi email, v.v.
            }
            SetErrorMessage("No products selected for purchase.");
            return this.Page();
        }
        private void SetSuccessMessage(string message)
        {
            TempData["SuccessMessage"] = message;
        }

        private void SetErrorMessage(string message)
        {
            TempData["ErrorMessage"] = message;
        }
    }
}
