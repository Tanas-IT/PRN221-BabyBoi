using BaByBoi.DataAccess.DTOs;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.DataAccess.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.Admin
{
    public class UpdateProductRealTimeModel : PageModel
    {
        private readonly IProductService _productService;

        public UpdateProductRealTimeModel(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> OnGetAsync(int productId)
        {
            if (productId <= 0)
            {
                return BadRequest("Invalid product ID");
            }
            var product = await _productService.GetProductByProductId(productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            var productDto = new ProductDTO
            {
                ProductCode = product.ProductCode,
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Discount = product.Discount,
                CategoryId = product.CategoryId,
                SizeId = product.ProductSizes.FirstOrDefault()?.SizeId,
                ImageUrl = product.ProductImages.FirstOrDefault()!.ImageUrl,
                Price = product.ProductSizes.FirstOrDefault()!.Price?.ToVietnameseCurrency(),
                DiscountedPrice = product.Discount.HasValue
                    ? ((product.ProductSizes.FirstOrDefault()?.Price ?? 0) * (1 - product.Discount.Value / 100.0)).ToVietnameseCurrency()
                    : (product.ProductSizes.FirstOrDefault()?.Price?.ToVietnameseCurrency() ?? "0")
            };
            return new JsonResult(productDto);
        }
    }
}
