using BaByBoi.Domain.BusinessModel;
using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<List<Product>> GetAllProductsAsync();
        public Task<List<Product>> GetAllProductsPagingAsync(int pageIndex, int pageSize);
        public Task<List<Product>> SearchProduct(string searchValue);
        public Task<List<Category>> GetAllCagetory();
        public Task<List<Size>> GetAllSize();
        public Task<Product> AddProduct(Product product);
        public Task<bool> AddImagesAndSize(Product product, List<ProductImage> productImagesLink, List<ProductSize> productSize, List<ProductSize> oldProductSize);

        public Task<List<ProductImage>> GetImages();
        public Task<Product> GetProductByProductId(int productId);
        public Task<bool> RemoveImagesAndSize(int productId);
        public Task<Product> UpdateProduct(Product UpdateProduct);
        public Task<bool> DeleteProduct(int productId);
        public Task<List<Product>> Search(string searchValue);
        
        public Task<List<ProductSize>> getProductSize(string searchValue);

        public Task<ProductSize> GetProductsSizesBySpecificSizeAsync(int productId, int sizeId);
        public Task<List<BarChartModel>> GetProductsForStatistic();
        Task<List<ProductSize>> GetProductSizeByProductId(int ProductId);

        Task<List<ProductSize>> GetProductByCategoryId(int categoryId);

    }
}
