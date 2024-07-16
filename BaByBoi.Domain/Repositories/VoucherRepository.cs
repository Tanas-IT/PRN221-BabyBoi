using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        private readonly BaByBoiContext _context;
        public VoucherRepository(BaByBoiContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Voucher> GetByCode(string code)
        {
            var voucher = await _context.Vouchers.Where(x => x.VoucherCode.Equals(code)).FirstOrDefaultAsync();
            return voucher!;
        }
    }
}
