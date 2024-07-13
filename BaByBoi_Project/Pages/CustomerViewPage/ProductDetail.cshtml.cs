using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace BaByBoi_Project.Pages.CustomerViewPage
{
    public class ProductDetailModel : PageModel
    {
        private readonly IProductService _productService;

        public ProductDetailModel(IProductService productService)
        {
            _productService = productService;
        }

        public Product Product { get; set; } = new Product();
        
        public User Customer { get; set; }

        public List<Product> recomendProduct = new List<Product>();
        public async Task OnGet(int productId, int categoryID)
        {
            await LoadDataAsync(productId: productId, categoryID: categoryID);
        }

        public async Task LoadDataAsync(int productId, int categoryID)
        {
            if (productId > 0)
            {
                Product = await _productService.GetProductByProductId(productId);
            }
            if (categoryID > 0)
            {
                var rcm = await _productService.GetAllProduct();
                recomendProduct = rcm.ToList();
            }

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
        }
        public async Task<IActionResult> OnGetAddToCartAsync(int productId, int sizeId, int categoryId)
        {
            List<OrderDetail> OrderList = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("OrderList") ?? new List<OrderDetail>();
            var product = await _productService.getProductById(productId);
            var productSize = await _productService.GetProductsSizesBySpecificSizeAsync(productId, sizeId);
            var orderDetail = new OrderDetail
            {
                ProductId = productSize.ProductId,
                Price = Math.Ceiling((double)(productSize.Price - (productSize.Price * product.Discount) / 100)!),
                Quantity = 1,
                Product = product,
                SizeId = productSize.SizeId,
                ProductSize = productSize

            };

            if (OrderList.Where(x => x.ProductId == orderDetail.ProductId && x.ProductSize.SizeId == orderDetail.SizeId).FirstOrDefault() == null)
            {
                OrderList.Add(orderDetail);
                HttpContext.Session.SetObjectAsJson("OrderList", OrderList);
            }
            await LoadDataAsync(productId:productId, categoryID: categoryId);
            return this.Page();
        }
    }
}
