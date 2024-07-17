using BaByBoi.DataAccess.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service.VNpayService
{
    public interface IVnpayService
    {
        string CreatePaymentUrl(HttpContext context, VNPaymentRequestModel model, int paymentID, double totalPrice, int voucherId);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
