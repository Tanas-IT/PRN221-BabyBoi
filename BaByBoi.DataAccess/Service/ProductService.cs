using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.BusinessModel;
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

        public async Task<List<Category>> GetAllCagetory()
        {
            return await _unitOfWork.ProductRepository.GetAllCagetory();
        }
        public async Task<List<ProductSize>> GetProductSizeByProductId(int ProductId)
        {
            return await _unitOfWork.ProductRepository.GetProductSizeByProductId(ProductId);
        }

        public async Task<List<Size>> GetAllSize()
        {
            return await _unitOfWork.ProductRepository.GetAllSize();
        }
        public async Task<bool> AddImagesAndSize(Product product, List<ProductImage> productImagesLink, List<ProductSize> productSize, List<ProductSize> oldProductSize)
        {
            return await _unitOfWork.ProductRepository.AddImagesAndSize(product, productImagesLink, productSize, oldProductSize);
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

        public async Task<List<BarChartModel>> GetProductsForStatistic()
        {
            return await _unitOfWork.ProductRepository.GetProductsForStatistic();
        }

        public async Task<List<ProductSize>> GetProductInCategoryId(int CategoryId)
            => await _unitOfWork.ProductRepository.GetProductByCategoryId(categoryId: CategoryId);

    }
}