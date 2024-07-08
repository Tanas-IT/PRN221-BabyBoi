using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.Admin
{
    public class BaseLayoutAdminModel : PageModel
    {
        private readonly IUserService _userService;

        public BaseLayoutAdminModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public User CurrentUser { get; set; }
        public void OnGet()
        {
            var tempUser = HttpContext.Session.GetObjectFromJson<User>("User");
            if (tempUser != null)
            {
                CurrentUser = tempUser;
            }
        }
       
    }
}
