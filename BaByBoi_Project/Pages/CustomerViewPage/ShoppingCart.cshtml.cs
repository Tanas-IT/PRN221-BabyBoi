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

        public void OnGet()
        {
            getCustomerFromSession();
            getShoppingCartFromSession();
            LoadMessagesFromTempData();
        }
        public IActionResult OnPostDeleteInCart(int ProductId, int SizeId)
        {
            getShoppingCartFromSession();
            var product = OrderList.FirstOrDefault(x => x.ProductId == ProductId && x.SizeId == SizeId);
            if (product != null)
            {
                OrderList.Remove(product);
            }
            SaveBookingListToSession();
            SetSuccessMessage("Room deleted successfully.");
            return this.Page();
        }
        private void SaveBookingListToSession()
        {
            HttpContext.Session.SetObjectAsJson("OrderList", OrderList);
        }
        public IActionResult OnPostUpdateInCart(int ProductId, string Quantity, int SizeId)
        {
            getShoppingCartFromSession();
            var product = OrderList.FirstOrDefault(x => x.ProductId == ProductId && x.SizeId == SizeId);
            if (product != null)
            {
                //product.Price = Quantity * product.Price;
                product.Quantity = product.ProductSize.Quantity > int.Parse(Quantity) ? int.Parse(Quantity) : product.ProductSize.Quantity;
                SaveBookingListToSession();
                SetSuccessMessage("Cập nhật giỏ hàng thành công");
            }
            else
            {
                SetErrorMessage("Không tìm thấy sản phẩm này.");
            }
            return this.Page();
        }

        public IActionResult UpdateTextQuantity(int ProductId, string quantities, int SizeId)
        {
            getShoppingCartFromSession();
            var product = OrderList.FirstOrDefault(x => x.ProductId == ProductId && x.SizeId == SizeId);
            if (product != null)
            {
                //product.Price = Quantity * product.Price;
                product.Quantity = product.ProductSize.Quantity > int.Parse(quantities) ? int.Parse(quantities) : product.ProductSize.Quantity;
                SaveBookingListToSession();
                SetSuccessMessage("Cập nhật giỏ hàng thành công");
            }
            else
            {
                SetErrorMessage("Không tìm thấy sản phẩm này.");
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
