using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi_Project.Extensions;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Common.Enum;

namespace BaByBoi_Project.Pages.LoginPage
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string email { get; set; } = null!;

        [BindProperty]
        public string password { get; set; } = null!;

        [BindProperty]
        public string ErrorMessage { get; set; } = null!;

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetObjectFromJson<User>("User") != null)
            {
                return RedirectToPage("/CustomerViewPage/CusViewProduct");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostLogin()
        {
            if (email == null)
            {
                ErrorMessage = "Email không được để trống.";
                return Page();
            }
            if (password == null)
            {
                ErrorMessage = "Mật khẩu không được để trống.";
                return Page();
            }
            var user = await _userService.CheckLogin(email!, password!);
            if (user != null)
            {
                HttpContext.Session.SetObjectAsJson("User", user);
                if (user.RoleId == (int)UserRole.Admin)
                {
                    return Content("Đây là trang admin nhé.");
                }
                else
                {
                    return RedirectToPage("/CustomerViewPage/CusViewProduct");
                }
            }
            else
            {
                ErrorMessage = "Email hoặc mật khẩu không hợp lệ.";
                return Page();
            }
        }


        public IActionResult OnGetGoogleLogin(string returnUrl = null!)
        {
            var redirectUrl = Url.Page("/LoginPage/Index", pageHandler: "GoogleCallback", values: new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> OnGetGoogleCallbackAsync(string returnUrl = null!)
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();

            if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
            {
                return RedirectToPage("./Login");
            }

            var claims = authenticateResult.Principal.Claims;

            var fullName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _userService.GetUserByEmail(email!);
            if (user == null)
            {
                user = new User
                {
                    Email = email!,
                    FullName = fullName ?? string.Empty,
                };
                var result = await _userService.AddAsync(user);
                if (result)
                {
                    user = await _userService.GetUserByEmail(email!);
                }
            }
            HttpContext.Session.SetObjectAsJson("User", user);
            if (user.RoleId == (int)UserRole.Admin)
            {
                return Content("Đây là trang admin nhé.");
            }
            else
            {
                return RedirectToPage("/CustomerViewPage/CusViewProduct");
            }
        }

        public IActionResult OnPostLogout(string returnUrl = null!)
        {
            HttpContext.Session.Clear();
            TempData.Clear();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToPage("/Index");
        }
    }
}
