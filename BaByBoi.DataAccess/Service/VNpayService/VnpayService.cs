using BaByBoi.DataAccess.Helper;
using BaByBoi.DataAccess.Utils;
using BaByBoi.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BaByBoi.DataAccess.Service.VNpayService
{
    public class VnpayService : IVnpayService
    {
        private readonly IConfiguration _config;

        public VnpayService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string CreatePaymentUrl(HttpContext context, VNPaymentRequestModel model, int paymentID, double totalPrice)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", _config["Vnpayment:Version"]!);
            vnpay.AddRequestData("vnp_Command", _config["Vnpayment:Command"]!);
            vnpay.AddRequestData("vnp_TmnCode", _config["Vnpayment:TmnCode"]!);
            vnpay.AddRequestData("vnp_Amount", (model.TotalPrice * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["Vnpayment:CurrCode"]!);
            vnpay.AddRequestData("vnp_IpAddr", VnpayHelper.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["Vnpayment:Locale"]!);
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + model.OrderCode);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            var returnUrl = $"{_config["Vnpayment:PaymentBackReturnUrl"]}?handler=PlaceOrder&paymentID={paymentID}&totalPrice={totalPrice}&orderCode={model.OrderCode}";
            vnpay.AddRequestData("vnp_ReturnUrl", returnUrl);

            vnpay.AddRequestData("vnp_TxnRef", model.OrderCode); // Use OrderCode as the transaction reference

            var paymentUrl = vnpay.CreateRequestUrl(_config["VnPayment:BaseUrl"]!, _config["VnPayment:HashSecret"]!);

            return paymentUrl;
        }



        public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();

           
            foreach (var item in collections)
            {
                if(!string.IsNullOrEmpty(item.Key) && item.Key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(item.Key, item.Value.ToString());
                }
            }
            var vnp_OrderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash!, _config["Vnpayment:HashSecret"]!);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false,
                };
            }
            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod =  "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_OrderId.ToString(),
                TransactionId = DateTime.Now.Ticks.ToString(),
                Token = vnp_SecureHash!,
                VnPayResponseCode = vnp_ResponseCode.ToString()
            };
        }
    }
}
