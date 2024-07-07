using System;
using System.Globalization;
using System.Threading.Tasks;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Common.Enum;
using BaByBoi_Project.Extensions;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BaByBoi_Project.Pages.RegisterPage
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
        public User User { get; set; }

        [BindProperty]
        public string VerificationCode { get; set; } = null!;

        [BindProperty]
        public long CountdownEndTime { get; set; }

        [BindProperty]
        public string ErrorMessage { get; set; } = null!;

        public void OnGet()
        {
            User = new User();
            User.Gender = 1;
        }

        public async Task<IActionResult> OnPostSendVerificationCodeAsync()
        {
            var emailExist = await _userService.GetUserByEmail(User.Email);
            if (emailExist != null)
            {
                return new JsonResult(new { success = false, error = "Email đã được đăng ký." });
            }

            if (User.Email == null)
            {
                return new JsonResult(new { success = false, error = "Email không được để trống." });
            }

            var random = new Random();
            VerificationCode = random.Next(100000, 999999).ToString();

            // Store the verification code in session or another temporary storage
            HttpContext.Session.SetString("VerificationCode", VerificationCode);

            // Send the verification code via email
            await _emailService.SendEmailAsync(User.Email, "Mã Xác Nhận Của Bạn", $"Mã xác nhận của bạn là {VerificationCode}");

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostRegister(string BirthDate)
        {
            var isError = false;

            var storedVerificationCode = HttpContext.Session.GetString("VerificationCode");
            if (VerificationCode != storedVerificationCode)
            {
                ModelState.AddModelError("VerificationCode", "Mã xác nhận không đúng");
                isError = true;
            }

            string[] formats = { "dd/MM/yyyy", "d/M/yyyy" };
            if (!DateTime.TryParseExact(BirthDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedUserBirthday))
            {
                ModelState.AddModelError("User.BirthDate", "Ngày sinh không hợp lệ");
                isError = true;
            }
            else
            {
                User.BirthDate = parsedUserBirthday;
            }

            if (User.Password!.Length < 6)
            {
                ModelState.AddModelError("User.Password", "Mật khẩu phải có ít nhất 6 ký tự");
                isError = true;
            }

            if (isError)
            {
                return Page();
            }

            User.CreateDate = DateTime.Now;
            User.Status = (int)StatusExist.Exist;
            User.RoleId = (int)UserRole.Customer;

            var result = await _userService.AddAsync(User);
            if (result)
            {
                var user = await _userService.GetUserByEmail(User.Email);
                HttpContext.Session.SetObjectAsJson("User", user);
                return RedirectToPage("/CustomerViewPage/CusViewProduct");
            }

            return RedirectToPage("/LoginPage");
        }
    }
}
