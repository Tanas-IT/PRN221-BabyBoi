// CreateModel.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;
using System.Threading.Tasks;

namespace BaByBoi_Project.Pages.VoucherPage
{
    public class CreateModel : PageModel
    {
        private readonly IVoucherService _voucherService;

        public CreateModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = new Voucher();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _voucherService.CreateVoucher(Voucher);

            return RedirectToPage("./Index");
        }
    }
}
