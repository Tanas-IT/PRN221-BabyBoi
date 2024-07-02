using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BaByBoi.Domain.Models;

namespace BaByBoi_Project.Pages.Admin.Category
{
    public class CreateModel : PageModel
    {
        private readonly BaByBoi.Domain.Models.BaByBoiContext _context;

        public CreateModel(BaByBoi.Domain.Models.BaByBoiContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public Cagetory Cagetory { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Cagetories == null || Cagetory == null)
            {
                return Page();
            }

            _context.Cagetories.Add(Cagetory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
