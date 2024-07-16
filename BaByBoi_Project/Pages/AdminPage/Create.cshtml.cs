using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class CreateModel : PageModel
    {
        private readonly IUserService _userService;

        public CreateModel(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> OnGet()
        {
            var roles = await _userService.GetAllRoles();
            ViewData["RoleId"] = new SelectList(roles, nameof(Role.RoleId), nameof(Role.RoleName));
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = new User();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["RoleId"] = new SelectList(await _userService.GetAllRoles(), "RoleId", "RoleName");
                return Page();
            }
            // Check if email already exists
            var existingUser = await _userService.GetUserByEmail(User.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("User.Email", "Email đã được đăng kí.");
                return Page();
            }
            User.CreateDate = DateTime.Now;
            User.UpdateDate = DateTime.Now;
            User.Status = 1;
            await _userService.CreateAsync(User);

            return RedirectToPage("/AdminPage/Index");
        }
    }
}
