using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;

namespace BaByBoi_Project.Pages.Category
{
    public class EditModel : PageModel
    {
        private readonly BaByBoiContext _context;

        public EditModel(BaByBoiContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BaByBoi.Domain.Models.Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryToUpdate = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == Category.CategoryId);

            if (categoryToUpdate == null)
            {
                return NotFound();
            }

            // Update the properties
            categoryToUpdate.CategoryCode = Category.CategoryCode;
            categoryToUpdate.CategoryName = Category.CategoryName;
            categoryToUpdate.CreateDate = Category.CreateDate;
            categoryToUpdate.UpdateDate = Category.UpdateDate;
            categoryToUpdate.UpdateBy = Category.UpdateBy;
            categoryToUpdate.CreateBy = Category.CreateBy;
            categoryToUpdate.Status = Category.Status;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
