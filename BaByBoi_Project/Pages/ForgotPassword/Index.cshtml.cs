using BaByBoi.DataAccess.Service.Interface;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BaByBoi_Project.Pages.ForgotPassword
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public IndexModel(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [BindProperty]
        public string Email { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var emailExist = await _userService.GetUserByEmail(Email);
            if (emailExist != null)
            {
                var random = new Random();
                var verificationCode = random.Next(100000, 999999).ToString();

                // Lưu mã xác nhận vào session
                HttpContext.Session.SetString("ResetPasswordVerificationCode", verificationCode);
                HttpContext.Session.SetString("EmailForgotPass", Email);
                var resetLink = $"https://localhost:7041/ResetPassword?token={verificationCode}";
                await _emailService.SendEmailAsync(Email, "Đặt Lại Mật Khẩu", $"Nhấp vào đây để đặt lại mật khẩu: <a href='{resetLink}'>{resetLink}</a>");
                TempData["SuccessMessage"] = "Một email đã được gửi đến địa chỉ của bạn để đặt lại mật khẩu.";
            }
            else
            {
                ModelState.AddModelError("Email", "Email chưa được đăng ký");
            }
            return Page();
        }
    }
}
