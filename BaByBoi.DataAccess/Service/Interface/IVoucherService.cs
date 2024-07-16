using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service.Interface
{
    public interface IVoucherService
    {
        public Task<IEnumerable<Voucher>> GetAll();
        Task<Voucher> GetVoucherById(int id);
        Task UpdateVoucher(Voucher voucher);
        Task DeleteVoucher(int id);
        Task CreateVoucher(Voucher voucher);
        Task<Voucher> getVoucherByCode(string code);
    }
}
