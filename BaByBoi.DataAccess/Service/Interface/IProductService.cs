using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service.Interface
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProduct();
        public Task<List<Product>> SearchProduct(string searchValue);
        public Task<List<Product>> GetAllProductsPagingAsync(int pageIndex, int pageSize);
        public Task<List<Cagetory>> GetAllCagetory();
        public Task<List<Size>> GetAllSize();
        public Task<bool> AddImagesAndSize(Product product, List<ProductImage> productImagesLink, List<ProductSize> productSize);
        public Task<Product> AddProduct(Product product);
        public Task<Product> UpdateProduct(Product product);
        public Task<Product> GetProductByProductId(int productId);
        public Task<bool> RemoveImagesAndSize(int productId);
        public Task<List<ProductImage>> GetImages();
        public Task<bool> DeleteProduct(int productId);
        Task<Product> getProductById(int id);
        Task<IEnumerable<Product>> getAllProduct();
        Task<Product> Insert(Product entity);
        Task<Product> update(Product entityUpdate);
        Task<bool> deleteById(int id);
        Task<List<Product>> Search(string searchKey);
        Task<List<ProductSize>> getProductWithSize(string searchKey);

        Task<ProductSize> GetProductsSizesBySpecificSizeAsync(int productId, int sizeId);


    }
}