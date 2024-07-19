using BaByBoi.Domain.Models;
using BusinessObject.IService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CategoryService : ICategoryService
{
    private readonly BaByBoiContext _context;

    public CategoryService(BaByBoiContext context)
    {
        _context = context;
    }

    public async Task<(List<Category> Categories, int TotalCount)> GetPaginatedCategoriesAsync(string searchString, int pageNumber, int pageSize)
    {
        var query = _context.Categories.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(c => c.CategoryName.Contains(searchString));
        }

        int totalCount = await query.CountAsync();

        var categories = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (categories, totalCount);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> GetCategoryByCode(string categoryCode)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryCode == categoryCode);
    }

    public async Task AddCategoryAsync(Category category)
    {
        _context.Categories.Add(category);
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
