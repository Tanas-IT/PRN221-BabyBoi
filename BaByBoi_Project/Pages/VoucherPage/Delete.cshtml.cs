using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;

namespace BaByBoi_Project.Pages.VoucherPage
{
    public class DeleteModel : PageModel
    {
        private readonly IVoucherService _voucherService;

        public DeleteModel(IVoucherService voucherService)
        {
            _voucherService = voucherService ?? throw new ArgumentNullException(nameof(voucherService));
        }

        [BindProperty]
        public Voucher Voucher { get; set; }

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
            }
            catch (Exception)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                await _voucherService.DeleteVoucher(id.Value);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
