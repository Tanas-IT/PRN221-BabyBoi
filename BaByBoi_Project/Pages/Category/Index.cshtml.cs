using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;

namespace BaByBoi_Project.Pages.Category
{
    public class IndexModel : PageModel
    {
        private readonly BaByBoiContext _context;

        public IndexModel(BaByBoiContext context)
        {
            _context = context;
        }

        public PaginatedList<BaByBoi.Domain.Models.Category> Categories { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchValue { get; set; } = string.Empty;

        public async Task OnGetAsync(int? pageIndex)
        {
            var categoriesQuery = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(SearchValue))
            {
                categoriesQuery = categoriesQuery.Where(c => c.CategoryName.Contains(SearchValue));
            }

            int pageSize = 5; // Number of items per page
            Categories = PaginatedList<BaByBoi.Domain.Models.Category>.Create(categoriesQuery, pageIndex ?? 1, pageSize);
        }

        public async Task<IActionResult> OnPostSearchAsync(string searchValue)
        {
            SearchValue = searchValue ?? string.Empty;
            int pageSize = 5;
            var categoriesQuery = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(SearchValue))
            {
                categoriesQuery = categoriesQuery.Where(c => c.CategoryName.Contains(SearchValue));
            }

            Categories =  PaginatedList<BaByBoi.Domain.Models.Category>.Create(categoriesQuery, 1, pageSize);
            return Page();
        }

        public async Task<IActionResult> OnPostResetAsync()
        {
            SearchValue = string.Empty;
            return RedirectToPage();
        }
    }
}
