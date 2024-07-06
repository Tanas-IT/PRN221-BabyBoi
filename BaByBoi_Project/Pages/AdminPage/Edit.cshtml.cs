using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;

        public EditModel(IUserService userService)
        {
            _userService = userService;
        }


        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            User = user;
                ViewData["RoleId"] = new SelectList(await _userService.GetAllRoles(), "RoleId", "RoleName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _userService.UpdateAsync(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _userService.UserExistsAsync(User.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/AdminPage/Index");
        }
    }
}
