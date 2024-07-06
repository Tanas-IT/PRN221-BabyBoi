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
        public string ErrorMessage { get; set; } = null!;

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetObjectFromJson<User>("User") != null)
            {
                return RedirectToPage("/CustomerViewPage/CusViewProduct");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostLogin(string email, string password)
        {
            var user = _userService.CheckLogin(email, password);
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

            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            return Content($"Login Google thành công. User: {name}, Email: {email}, UserId: {userId}");
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
