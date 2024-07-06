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
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Product>> GetAllProduct()
        {
           return  await _unitOfWork.ProductRepository.GetAllProductsAsync(); 
        }
        public async Task<List<Product>> SearchProduct(string searchValue)
        {
            return await _unitOfWork.ProductRepository.SearchProduct(searchValue);
        }
        public async Task<List<Product>> GetAllProductsPagingAsync(int pageIndex, int pageSize)
        {
            return await _unitOfWork.ProductRepository.GetAllProductsPagingAsync(pageIndex, pageSize);
        }

        public async Task<List<Cagetory>> GetAllCagetory()
        {
            return await _unitOfWork.ProductRepository.GetAllCagetory();
        }
        public async Task<List<Size>> GetAllSize()
        {
            return await _unitOfWork.ProductRepository.GetAllSize();
        }
        public async Task<bool> AddImagesAndSize(Product product, List<ProductImage> productImagesLink, List<ProductSize> productSize)
        {
            return await _unitOfWork.ProductRepository.AddImagesAndSize(product, productImagesLink, productSize);
        }
        public async Task<Product> AddProduct(Product product)
        {
            return await _unitOfWork.ProductRepository.AddProduct(product);
        }

        public async Task<List<ProductImage>> GetImages()
        {
            return await _unitOfWork.ProductRepository.GetImages();
        }
        public async Task<Product> GetProductByProductId(int productId)
        {
            return await _unitOfWork.ProductRepository.GetProductByProductId(productId);
        }
        public async Task<bool> RemoveImagesAndSize(int productId)
        {
            return await _unitOfWork.ProductRepository.RemoveImagesAndSize(productId);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            return await _unitOfWork.ProductRepository.UpdateProduct(product);
        }
        public async Task<bool> DeleteProduct(int productId)
        {
            return await _unitOfWork.ProductRepository.DeleteProduct(productId);
        }

    }
}
