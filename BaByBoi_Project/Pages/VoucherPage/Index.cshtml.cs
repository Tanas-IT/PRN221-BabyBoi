using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BaByBoi.Domain.Models;
using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.PaginModel;

namespace BaByBoi_Project.Pages.VoucherPage
{
    public class IndexModel : PageModel
    {
        private readonly IVoucherService _voucherService;

        public IndexModel(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        public PaginatedList<Voucher> Voucher { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            var voucher = await _voucherService.GetAll();

            int pageSize = 10;
            Voucher =  PaginatedList<Voucher>.Create(voucher.AsQueryable(), pageIndex ?? 1, pageSize);
        }
    }
}
