using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.ResetPassword
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        [BindProperty]
        public string NewPassword { get; set; } = null!;

        [BindProperty]
        public string ConfirmPassword { get; set; } = null!;

        public IActionResult OnGet()
        {
            var tokenReq = Request.Query["token"].ToString();
            var token = HttpContext.Session.GetObjectFromJson<string>("ResetPasswordVerificationCode");
            if (token == null)
            {
                return RedirectToPage("/Guest/LoginPage/Index");
            }
            if (tokenReq != token)
            {
                TempData["ErrorMessage"] = "Mã xác nhận không chính xác.";
                return RedirectToPage("/Guest/ForgotPassword/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var isError = false;

            if (NewPassword.Length < 6)
            {
                ModelState.AddModelError("NewPassword", "Mật khẩu phải có ít nhất 6 ký tự");
                isError = true;
            }

            if (ConfirmPassword != NewPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Xác nhận mật khẩu không khớp với mật khẩu mới");
                isError = true;
            }

            if (isError)
            {
                return Page();
            }
            var email = HttpContext.Session.GetString("EmailForgotPass");
            var user = await _userService.ChangePasswordByEmail(email!, NewPassword);
            if (user != null)
            {
                HttpContext.Session.Remove("EmailForgotPass");
                HttpContext.Session.Remove("ResetPasswordVerificationCode");
                return RedirectToPage("/Guest/LoginPage/Index");
            }
            return Page();
        }

    }
}
