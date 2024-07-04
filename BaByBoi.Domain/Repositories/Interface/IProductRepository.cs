using BaByBoi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<List<Product>> Search(string searchValue);
        
        public Task<List<ProductSize>> getProductSize(string searchValue);

        public Task<ProductSize> GetProductsSizesBySpecificSizeAsync(int productId, int sizeId);
    }
}
