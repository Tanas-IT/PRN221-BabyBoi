using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service
{
    public class ProductService : IProductService
    {
        public readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> deleteById(int id)
        {
            var getProduct = await _unitOfWork.ProductRepository.GetById(id);
            var result = false;
            if (getProduct != null)
            {
                result = await _unitOfWork.ProductRepository.RemovebyId(id);
                result = true;
            }
            return result;
        }

        public async Task<IEnumerable<Product>> getAllProduct()
        {
            var products = await _unitOfWork.ProductRepository.GetAll();
            return products;
        }

        public async Task<Product> getProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetById(id);
            return product;
        }

        public async Task<Product> Insert(Product entity)
        {
            var result = false;
            result = await _unitOfWork.ProductRepository.AddAsync(entity);
            if (result)
            {
                return entity;
            }
            return null!;
        }

        public async Task<List<Product>> Search(string searchKey) 
            => await _unitOfWork.ProductRepository.Search(searchKey);
        

        public async Task<Product> update(Product entityUpdate)
        {
            var result = await _unitOfWork.ProductRepository.UpdateAsync(entityUpdate);
            if (result)
            {
                return entityUpdate;
            }
            return null!;

        }
        public async Task<List<ProductSize>> getProductWithSize(string searchKey)
        => await _unitOfWork.ProductRepository.getProductSize(searchKey);

        public async Task<ProductSize> GetProductsSizesBySpecificSizeAsync(int productId, int sizeId)
        => await _unitOfWork.ProductRepository.GetProductsSizesBySpecificSizeAsync(productId, sizeId);
    }
}
