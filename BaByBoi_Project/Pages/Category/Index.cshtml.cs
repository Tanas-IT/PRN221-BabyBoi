using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;

namespace BaByBoi_Project.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly BaByBoiContext _context;

        public IndexModel(BaByBoiContext context)
        {
            _context = context;
        }

        public IList<BaByBoi.Domain.Models.Category> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Categories != null)
            {
                Category = await _context.Categories.ToListAsync();
            }
        }
    }
}
