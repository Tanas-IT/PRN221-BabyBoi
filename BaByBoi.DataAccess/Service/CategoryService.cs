using BaByBoi.Domain.Models;
using BusinessObject.IService;
using DataAccess.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObject.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<(List<Cagetory> Categories, int TotalCount)> GetPaginatedCategoriesAsync(string searchString, int pageNumber, int pageSize)
        {
            return await _categoryRepository.GetPaginatedCategoriesAsync(searchString, pageNumber, pageSize);
        }

        public async Task<Cagetory> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task AddCategoryAsync(Cagetory category)
        {
            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(Cagetory category)
        {
            await _categoryRepository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteCategoryAsync(id);
        }
    }
}
