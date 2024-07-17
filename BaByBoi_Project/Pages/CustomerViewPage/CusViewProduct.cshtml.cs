using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public List<BaByBoi.Domain.Models.Category> Categories { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 5;

        [BindProperty(SupportsGet = true)]
        public string SearchValue { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortBy { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        public async Task OnGetAsync(string searchValue, string sortBy, int? categoryId, int pageIndex = 1)
        {
            await LoadDataAsync(searchValue, sortBy, categoryId, pageIndex);
        }

        public async Task LoadDataAsync(string searchValue, string sortBy, int? categoryId, int pageIndex)
        {
            var products = await _productService.getProductWithSize(searchValue);
            Categories = await _productService.GetAllCagetory();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.Product.CategoryId == categoryId.Value).ToList();
            }

            switch (sortBy)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.Product.ProductName).ToList();
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Product.ProductName).ToList();
                    break;
            }

            Products = PaginatedList<ProductSize>.Create(
                products.AsQueryable(), pageIndex, PageSize);

            SearchValue = searchValue;
            SortBy = sortBy;
            CategoryId = categoryId;
            PageIndex = pageIndex;

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            }
            if (TempData.TryGetValue("ErrorMessage", out var errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
        }

        public async Task<IActionResult> OnGetAddToCartAsync(int productId, int sizeId)
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
            await LoadDataAsync(SearchValue, SortBy, CategoryId, PageIndex);
            return this.Page();
        }

        public async Task<IActionResult> OnGetViewDetail(int productId, int categoryId)
        {
            var product = await _productService.GetProductByProductId(productId);
            return RedirectToPage("ProductDetail", new { productId = product.ProductId, categoryId });
        }

        public async Task OnGetSearchByCategory(int categoryId, string sortBy, string searchValue, int pageIndex = 1)
        {
            await LoadDataAsync(searchValue, sortBy, categoryId, pageIndex);
        }
    }
}
