using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;

namespace BaByBoi_Project.Pages.VoucherPage
{
    public class EditModel : PageModel
    {
        private readonly IVoucherService _voucherService;

        public EditModel(IVoucherService voucherService)
        {
            _voucherService = voucherService ?? throw new ArgumentNullException(nameof(voucherService));
        }

        [BindProperty]
        public Voucher Voucher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Voucher = await _voucherService.GetVoucherById(id.Value);
                if (Voucher == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _voucherService.UpdateVoucher(Voucher);
            }
            catch (Exception)
            {
                // Handle exception as needed
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
