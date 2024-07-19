using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class DeleteModel : PageModel
    {
        private readonly IUserService _userService;

        public DeleteModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User User { get; set; } = default!;
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _userService.GetByIdAsync(id.Value);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            bool result = await _userService.DeleteUserAsync(id.Value);

            if (!result)
            {
                ErrorMessage = "Không thể xóa người dùng. Vui lòng thử lại.";
                User = await _userService.GetByIdAsync(User.UserId); // Refresh the user data
                return Page();
            }

            return RedirectToPage("/AdminPage/Index");
        }
    }
}
