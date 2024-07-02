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

        public async Task<(List<Cagetory> Categories, int TotalCount)> GetPaginatedCategoriesAsync(string searchString, int pageNumber, int pageSize)
        {
            var query = _context.Cagetories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                query = query.Where(c => c.CagetoryName.Contains(searchString));
            }

            int totalCount = await query.CountAsync();
            List<Cagetory> categories = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return (categories, totalCount);
        }

        public async Task<Cagetory> GetCategoryByIdAsync(int id)
        {
            return await _context.Cagetories.FindAsync(id);
        }

        public async Task AddCategoryAsync(Cagetory category)
        {
            await _context.Cagetories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Cagetory category)
        {
            _context.Cagetories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Cagetories.FindAsync(id);
            if (category != null)
            {
                _context.Cagetories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
