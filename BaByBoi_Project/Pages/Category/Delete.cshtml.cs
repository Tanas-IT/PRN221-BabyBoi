using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;

namespace BaByBoi_Project.Pages.Category
{
    public class DeleteModel : PageModel
    {
        private readonly BaByBoiContext _context;

        public DeleteModel(BaByBoiContext context)
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

            Category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Category?.CategoryId == null)
            {
                return NotFound();
            }

            Category = await _context.Categories.FindAsync(Category.CategoryId);

            if (Category == null)
            {
                return NotFound();
            }

            // Check if there are products associated with this category
            bool hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == Category.CategoryId);

            if (hasProducts)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete this category because it is referenced by products.");
                return Page(); // Return the delete page with the error message
            }

            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
