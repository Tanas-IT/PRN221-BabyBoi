﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using BaByBoi.DataAccess.DTOs;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi_Project.Common.Enum;
using BaByBoi_Project.Extensions;
using FUMiniHotelManagement.Hubs;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace BaByBoi_Project.Pages.RegisterPage
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IHubContext<SignalrServer> _signalRHub;

        public IndexModel(IUserService userService, IEmailService emailService, IHubContext<SignalrServer> signalRHub)
        {
            _userService = userService;
            _emailService = emailService;
            _signalRHub = signalRHub;
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
            var verificationCodeJson = HttpContext.Session.GetObjectFromJson<VerificationCodeDTO>("VerificationCode");
            if (verificationCodeJson != null)
            {
                CountdownEndTime = ((DateTimeOffset)verificationCodeJson.ExpirationTime).ToUnixTimeMilliseconds();
            }
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
            var verificationCodeDTO = new VerificationCodeDTO
            {
                Code = VerificationCode,
                ExpirationTime = DateTime.Now.AddMinutes(5)
            };
            HttpContext.Session.SetObjectAsJson("VerificationCode", verificationCodeDTO);
            // Send the verification code via email
            await _emailService.SendEmailAsync(User.Email, "Mã Xác Nhận Của Bạn", $"Mã xác nhận của bạn là {VerificationCode}");

            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostRegister(string BirthDate)
        {
            var isError = false;

            var storedVerificationCode = HttpContext.Session.GetObjectFromJson<VerificationCodeDTO>("VerificationCode");
            if (DateTime.Now > storedVerificationCode.ExpirationTime)
            {
                ModelState.AddModelError("VerificationCode", "Mã xác nhận đã hết hạn.");
                isError = true;
            }
            else if (VerificationCode != storedVerificationCode.Code)
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
                int totalUser = await _userService.GetTotalUserAsync();
                await _signalRHub.Clients.All.SendAsync("ReceiveUserCount", totalUser);

                var user = await _userService.GetUserByEmail(User.Email);
                HttpContext.Session.SetObjectAsJson("User", user);
                HttpContext.Session.Remove("VerificationCode");
                return RedirectToPage("/CustomerViewPage/CusViewProduct");
            }

            return RedirectToPage("/Guest/LoginPage");
        }
    }
}
