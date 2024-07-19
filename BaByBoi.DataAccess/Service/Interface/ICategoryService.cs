using BaByBoi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessObject.IService
{
    public interface ICategoryService
    {
        Task<(List<Category> Categories, int TotalCount)> GetPaginatedCategoriesAsync(string searchString, int pageNumber, int pageSize);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> GetCategoryByCode(string categoryCode); // Add this method
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
