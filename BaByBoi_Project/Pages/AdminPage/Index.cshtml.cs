using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.PaginModel;
using BaByBoi.DataAccess.Service.Interface;

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

        public async Task OnGetAsync(int? pageIndex)
        {
            var users = await _userService.GetAll();

            int pageSize = 10; // Số mục trên mỗi trang
            User = PaginatedList<User>.Create(users.AsQueryable(), pageIndex ?? 1, pageSize);
        }
    }
}
