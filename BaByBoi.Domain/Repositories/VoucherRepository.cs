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
        public VoucherRepository(BaByBoiContext context) : base(context) { }

        //public override async Task<IEnumerable<Voucher>> GetAll()
        //{
        //    return await _context.Vouchers.ToListAsync() ?? new List<Voucher>();
        //}
    }
}
