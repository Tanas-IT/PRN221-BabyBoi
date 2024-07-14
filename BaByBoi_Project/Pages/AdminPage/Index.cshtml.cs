using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi.DataAccess.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace BaByBoi_Project.Pages.AdminPage
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        public PaginatedList<User> User { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchValue { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            if (SearchValue == null)
            {
                SearchValue = "";
            }
            var users = await _userService.SearchUser(SearchValue);

            int pageSize = 5; // Số mục trên mỗi trang
            User = PaginatedList<User>.Create(users.AsQueryable(), pageIndex ?? 1, pageSize);
        }
        public async Task<IActionResult> OnPostResetAccount()
        {
            await OnGetAsync(1);
            return Page();
        }
        public async Task<IActionResult> OnPostSearchAccount(string SearchValue)
        {
            if (SearchValue == null)
            {
                SearchValue = "";
            }
            int pageSize = 5;
            var users = await _userService.SearchUser(SearchValue);
            User = PaginatedList<User>.Create(users.AsQueryable(), 1 , pageSize);
            return Page();
        }
    }
}
