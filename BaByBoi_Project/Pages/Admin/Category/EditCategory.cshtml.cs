using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;

namespace BaByBoi_Project.Pages.Admin.Category
{
    public class EditModel : PageModel
    {
        private readonly BaByBoi.Domain.Models.BaByBoiContext _context;

        public EditModel(BaByBoi.Domain.Models.BaByBoiContext context)
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

            var cagetory =  await _context.Cagetories.FirstOrDefaultAsync(m => m.CagetoryId == id);
            if (cagetory == null)
            {
                return NotFound();
            }
            Cagetory = cagetory;
           ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Cagetory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CagetoryExists(Cagetory.CagetoryId))
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

        private bool CagetoryExists(int id)
        {
          return (_context.Cagetories?.Any(e => e.CagetoryId == id)).GetValueOrDefault();
        }
    }
}
