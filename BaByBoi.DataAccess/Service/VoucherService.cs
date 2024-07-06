using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service
{
    public class VoucherService : IVoucherService
    {
        public readonly IUnitOfWork _unitOfWork;

        public VoucherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
