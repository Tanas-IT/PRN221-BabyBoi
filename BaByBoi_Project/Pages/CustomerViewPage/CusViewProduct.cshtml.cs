using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace BaByBoi_Project.Pages.CustomerViewPage
{
    public class CusViewProductModel : PageModel
    {
        private readonly IProductService _productService;

        public CusViewProductModel(IProductService productService)
        {
            _productService = productService;
        }
        public PaginatedList<ProductSize> Products { get; private set; }
        public List<Category> Categories { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string SearchValue { get; set; }
        public async Task OnGetAsync(string searchValue)
        {
            await LoadDataAsync(searchValue);
        }

        public async Task LoadDataAsync(string? searchValue)
        {
            var products = await _productService.getProductWithSize(searchValue);
            this.Products = PaginatedList<ProductSize>.Create(
                products.AsQueryable(), PageIndex, PageSize);

            SearchValue = searchValue;

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int productId, int sizeId)
        {
            List<OrderDetail> OrderList = HttpContext.Session.GetObjectFromJson<List<OrderDetail>>("OrderList") ?? new List<OrderDetail>();
            var product = await _productService.getProductById(productId);
            var productSize = await _productService.GetProductsSizesBySpecificSizeAsync(productId, sizeId);
            var orderDetail = new OrderDetail
            {
                ProductId = productSize.ProductId,
                Price = Math.Ceiling((double)(productSize.Price -  (productSize.Price * product.Discount)/100)!),
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
            await LoadDataAsync(SearchValue);
            return this.Page();
        }

    }
}
