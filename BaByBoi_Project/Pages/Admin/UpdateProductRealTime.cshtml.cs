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
                ImageUrl = product.ProductImages.FirstOrDefault()!.ImageUrl,
                Price = product.ProductSizes.FirstOrDefault()!.Price?.ToVietnameseCurrency()
            };
            return new JsonResult(productDto);
        }
    }
}
