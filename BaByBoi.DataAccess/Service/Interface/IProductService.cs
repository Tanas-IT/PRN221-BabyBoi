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
