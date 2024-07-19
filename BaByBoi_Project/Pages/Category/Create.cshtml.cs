using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BusinessObject.IService;

namespace BaByBoi_Project.Pages.Category
{
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public BaByBoi.Domain.Models.Category Category { get; set; } = new BaByBoi.Domain.Models.Category();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if the category code already exists
            var existingCategory = await _categoryService.GetCategoryByCode(Category.CategoryCode);
            if (existingCategory != null)
            {
                ModelState.AddModelError("Category.CategoryCode", "Category Code đã được sử dụng.");
                return Page();
            }

            Category.CreateDate = DateTime.Now;
            Category.UpdateDate = DateTime.Now;
            // Set default values or logic for other properties if necessary
            Category.Status = 1; // Assuming 1 means Active

            await _categoryService.AddCategoryAsync(Category);

            return RedirectToPage("./Index");
        }
    }
}
