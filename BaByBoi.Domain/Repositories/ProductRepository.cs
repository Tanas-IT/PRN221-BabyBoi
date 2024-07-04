using BaByBoi.Domain.Common.Enum;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly BaByBoiContext _context;
        public ProductRepository(BaByBoiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductSize>> getProductSize(string searchValue)
        {
            var sizeID = await _context.Sizes.Where(x => x.SizeName!.Equals("Small")).Select(x => x.SizeId).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(searchValue))
            {
                var productSizes = await _context.ProductSizes
            //.Where(ps => ps.SizeId == sizeID)
            .Include(ps => ps.Product)
            .Include(ps => ps.Size)
            .ToListAsync();

                return productSizes.DistinctBy(ps => ps.ProductId).ToList();
            }
            else
            {
                var productSizes = await _context.ProductSizes
            .Where(ps =>  ps.Product.ProductName!.ToLower().Contains(searchValue.ToLower()))
            .Include(ps => ps.Product)
            .Include(ps => ps.Size)
            .ToListAsync();

                return productSizes.DistinctBy(ps => ps.ProductId).ToList();
                
            }
        }

        public async Task<ProductSize> GetProductsSizesBySpecificSizeAsync(int productId, int sizeId)
        {
            var productSize = await _context.ProductSizes.Where(x => x.ProductId == productId && x.SizeId == sizeId).FirstOrDefaultAsync();
            return productSize!;
        }

            public async Task<List<Product>> Search(string searchValue)
        {

            // building query
            var products = _context.Products.AsQueryable();


            var isNumber = false;

            // is integer number
            if (int.TryParse(searchValue, out var num))
            {
                // flag is number
                isNumber = true;

                products = products.Where(x => x.ProductId == num || x.ProductCode!.ToString().Contains(searchValue));
            }

            // Check if searchValue contains "Active" or "Inactive" or is string 
            if (!String.IsNullOrEmpty(searchValue) && !isNumber)
            {
                products = products.Where(x => (x.Status == (byte)ProductStatus.Active && "Active".Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                                             || (x.Status == (byte)ProductStatus.InActive && "InActive".Contains(searchValue, StringComparison.OrdinalIgnoreCase))
                                             || x.ProductName!.Contains(searchValue));

            }


            var result = await products.ToListAsync();

            return result;
        }
    }
}
