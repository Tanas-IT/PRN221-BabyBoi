using BaByBoi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<(List<Cagetory> Categories, int TotalCount)> GetPaginatedCategoriesAsync(string searchString, int pageNumber, int pageSize);
        Task<Cagetory> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Cagetory category);
        Task UpdateCategoryAsync(Cagetory category);
        Task DeleteCategoryAsync(int id);
    }
}
