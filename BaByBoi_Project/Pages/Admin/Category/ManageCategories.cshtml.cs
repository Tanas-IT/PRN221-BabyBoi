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
    public class IndexModel : PageModel
    {
        private readonly BaByBoi.Domain.Models.BaByBoiContext _context;

        public IndexModel(BaByBoi.Domain.Models.BaByBoiContext context)
        {
            _context = context;
        }

        public IList<Cagetory> Cagetory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Cagetories != null)
            {
                Cagetory = await _context.Cagetories
                .Include(c => c.User).ToListAsync();
            }
        }
    }
}
