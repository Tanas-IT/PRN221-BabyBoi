using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace BaByBoi_Project.Pages.LoginPage
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

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
    }
}
