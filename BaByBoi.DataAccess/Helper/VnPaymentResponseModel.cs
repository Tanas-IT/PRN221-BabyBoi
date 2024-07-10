using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Helper
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }

    public class VNPaymentRequestModel
    {
        public string OrderCode {  get; set; }
        public int UserID {  get; set; }
        public string Fullname { get; set; }
        public string Descriptuon { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
