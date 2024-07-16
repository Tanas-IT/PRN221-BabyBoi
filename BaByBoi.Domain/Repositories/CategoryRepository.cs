using BaByBoi.Domain.Models;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BaByBoiContext _context;

        public CategoryRepository(BaByBoiContext context)
        {
            _context = context;
        }

        public async Task<(List<Category> Categories, int TotalCount)> GetPaginatedCategoriesAsync(string searchString, int pageNumber, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c => c.CategoryName.Contains(searchString));
            }

            int totalCount = await query.CountAsync();
            List<Category> categories = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (categories, totalCount);
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
