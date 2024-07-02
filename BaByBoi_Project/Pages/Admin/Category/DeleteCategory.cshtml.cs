using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;

namespace BaByBoi_Project.Pages.Admin.Category
{
    public class DeleteModel : PageModel
    {
        private readonly BaByBoi.Domain.Models.BaByBoiContext _context;

        public DeleteModel(BaByBoi.Domain.Models.BaByBoiContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Cagetory Cagetory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Cagetories == null)
            {
                return NotFound();
            }

            var cagetory = await _context.Cagetories.FirstOrDefaultAsync(m => m.CagetoryId == id);

            if (cagetory == null)
            {
                return NotFound();
            }
            else 
            {
                Cagetory = cagetory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Cagetories == null)
            {
                return NotFound();
            }
            var cagetory = await _context.Cagetories.FindAsync(id);

            if (cagetory != null)
            {
                Cagetory = cagetory;
                _context.Cagetories.Remove(Cagetory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
