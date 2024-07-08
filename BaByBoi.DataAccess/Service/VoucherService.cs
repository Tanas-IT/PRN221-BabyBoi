using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service
{
    public class VoucherService : IVoucherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VoucherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Voucher>> GetAll()
        {
            return await _unitOfWork.VoucherRepository.GetAll();
        }

        public async Task<Voucher> GetVoucherById(int id)
        {
            return await _unitOfWork.VoucherRepository.GetById(id);
        }
        public async Task UpdateVoucher(Voucher voucher)
        {
            await _unitOfWork.VoucherRepository.Update(voucher);
        }

        public async Task DeleteVoucher(int id)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetById(id);
            if (voucher != null)
            {
                await _unitOfWork.VoucherRepository.Remove(voucher);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task CreateVoucher(Voucher voucher)
        {
            await _unitOfWork.VoucherRepository.Add(voucher);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
