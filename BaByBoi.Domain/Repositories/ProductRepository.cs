﻿using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaByBoi.Domain.Common.Enum;
using BaByBoi.Domain.BusinessModel;


namespace BaByBoi.Domain.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(BaByBoiContext context) : base(context) { }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .ToListAsync();
        }

        public async Task<List<Product>> SearchProduct(string searchValue)
        {
            int checkSearchInt = 0;
            bool checkSearch = int.TryParse(searchValue, out checkSearchInt);
            if (checkSearch)
            {
                return await _context.Products
                                 .Include(x => x.Category)
                                .Include(x => x.ProductImages)
                                .Include(x => x.ProductSizes)
                                .Where(x => x.ProductId == checkSearchInt || x.Discount == checkSearchInt || x.Status == checkSearchInt)
                                .ToListAsync();
            }
            else
            {
                DateTime checkDate = DateTime.Now;
                bool checkDateTime = DateTime.TryParse(searchValue, out checkDate);
                if (checkDateTime)
                {
                    return await _context.Products
                                   .Include(x => x.Category)
                               .Include(x => x.ProductImages)
                               .Include(x => x.ProductSizes)
                               .Where(x => x.CreateDate == checkDate)
                               .ToListAsync();
                }
                else
                {
                    return await _context.Products
                                   .Include(x => x.Category)
                                   .Include(x => x.ProductImages)
                                   .Include(x => x.ProductSizes)
                                   .Where(x => x.ProductCode.ToLower().Contains(searchValue.ToLower())
                                               || x.ProductName.ToLower().Contains(searchValue.ToLower())
                                               || x.Description.ToLower().Contains(searchValue.ToLower())
                                               || x.Category.CategoryName.ToLower().Contains(searchValue.ToLower())
                                               )
                                   .ToListAsync();
                }
            }
        }
        public async Task<List<Product>> GetAllProductsPagingAsync(int pageIndex, int pageSize)
        {
            int skip = (pageIndex - 1) * pageSize;
            List<Product> listProduct = await _context.Products
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductSizes)
                .ThenInclude(x => x.Size)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            return listProduct;
        }

        public Task<List<Category>> GetAllCagetory()
        {
            return _context.Categories.Include(x => x.Products).ToListAsync();
        }
        public Task<List<Size>> GetAllSize()
        {
            return _context.Sizes.ToListAsync();
        }

        public async Task<Product> AddProduct(Product product)
        {
            if (product != null)
            {
                _context.Products.Add(product);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    var productNew = _context.Products.OrderByDescending(x => x.ProductId).First();
                    return productNew;
                }
            }
            return null;
        }


        public async Task<bool> AddImagesAndSize(Product product, List<ProductImage> productImagesLink, List<ProductSize> productSize, List<ProductSize> oldProductSize)
        {
            if (product != null)
            {
                foreach (var productImage in productImagesLink)
                {
                    _context.ProductImages.Add(productImage);
                }

                foreach (var size in productSize)
                {
                    bool flag = false;
                    foreach (var oldSize in oldProductSize)
                    {
                        if (size.ProductId == oldSize.ProductId && size.SizeId == oldSize.SizeId)
                        {
                            flag = true;
                            break;
                        }

                    }
                    if (!flag)
                    {
                        _context.ProductSizes.Add(size);

                    }
                }
                var result = await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<List<ProductImage>> GetImages()
        {
            var listImages = await _context.ProductImages.ToListAsync();
            return listImages;
        }
        public async Task<Product> GetProductByProductId(int productId)
        {
            var product = await _context.Products
                                   .Include(x => x.Category)
                                   .Include(x => x.ProductImages)
                                   .Include(x => x.ProductSizes)
                                   .ThenInclude(x => x.Size)
                                   .FirstOrDefaultAsync(x => x.ProductId == productId);
            if (product != null)
            {
                return product;
            }
            return null;
        }
        public async Task<bool> RemoveImagesAndSize(int productId)
        {
            var listProdutImages = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();
            //var listProductSizes = await _context.ProductSizes.Where(x => x.ProductId == productId).ToListAsync();
            if (listProdutImages.Any())
            {
                _context.ProductImages.RemoveRange(listProdutImages);
            }
            //if(listProductSizes.Any()) 
            //{ 
            //    _context.ProductSizes.RemoveRange( listProductSizes); 
            //}
            var result = await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> UpdateProduct(Product UpdateProduct)
        {
            var oldProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == UpdateProduct.ProductId);
            if (oldProduct != null)
            {
                oldProduct.ProductCode = UpdateProduct.ProductCode;
                oldProduct.ProductName = UpdateProduct.ProductName;
                oldProduct.CreateBy = UpdateProduct.CreateBy;
                oldProduct.UpdateDate = DateTime.Now;
                oldProduct.UpdateBy = UpdateProduct.UpdateBy;
                oldProduct.Description = UpdateProduct.Description;
                oldProduct.Status = UpdateProduct.Status;
                oldProduct.CategoryId = UpdateProduct.CategoryId;
                oldProduct.Discount = UpdateProduct.Discount;
                await _context.SaveChangesAsync();
                return oldProduct;
            }
            return null;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var deleteProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == productId);
            if (deleteProduct != null)
            {
                deleteProduct.Status = 0;
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

        public async Task<List<ProductSize>> getProductSize(string searchValue)
        {
            if (string.IsNullOrEmpty(searchValue))
            {
                var productSizes = await _context.ProductSizes
            //.Where(ps => ps.SizeId == sizeID)
            .Include(x => x.Product)
            .Include(x => x.Product.ProductImages)
            .Include(ps => ps.Size)
            .Include(ps => ps.Product.ProductImages)
            .ToListAsync();

                return productSizes.DistinctBy(ps => ps.ProductId).ToList();
            }
            else
            {
                var productSizes = await _context.ProductSizes
            .Where(ps => ps.Product.ProductName!.ToLower().Contains(searchValue.ToLower()))
            .Include(ps => ps.Product)
            .Include(ps => ps.Size)
            .Include(ps => ps.Product.ProductImages)
            .ToListAsync();

                return productSizes.DistinctBy(ps => ps.ProductId).ToList();

            }
        }

        public async Task<ProductSize> GetProductsSizesBySpecificSizeAsync(int productId, int sizeId)
        {
            var productSize = await _context.ProductSizes
                .Include(x => x.Product)
                .ThenInclude(x => x.ProductImages)
                .Include(x => x.Size)
                .Where(x => x.ProductId == productId && x.SizeId == sizeId)
                .FirstOrDefaultAsync();
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
        public async Task<List<BarChartModel>> GetProductsForStatistic()
        {
            var products = await _context.Products.ToListAsync(); // Lấy danh sách các sản phẩm
            var categories = await _context.Categories.ToListAsync(); // Lấy danh sách các sản phẩm
            var listProduct = (from c in categories
                               select new BarChartModel()
                               {
                                   CategoryName = c.CategoryName,
                                   Amount = products.Count(p => p.CategoryId == c.CategoryId)
                               }).ToList();
            return listProduct;
        }

        public async Task<List<ProductSize>> GetProductSizeByProductId(int ProductId)
        {
            return await _context.ProductSizes.Where(x => x.ProductId == ProductId).ToListAsync();
        }

        public async Task<List<ProductSize>> GetProductByCategoryId(int categoryId)
        {
            var productSizes = await _context.ProductSizes
            .Include(x => x.Product)
            .Include(x => x.Product.ProductImages)
            .Include(ps => ps.Size)
            .Include(ps => ps.Product.ProductImages)
            .Where(x => x.Product.CategoryId == categoryId)
            .ToListAsync();

            return productSizes.DistinctBy(ps => ps.ProductId).ToList();

        }
    }
}