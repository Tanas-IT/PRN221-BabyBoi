using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public string? OrderDate { get; set; }
        public string? OrderCode { get; set; }
        public int? UserId { get; set; }
        public string? TotalPrice { get; set; }
        public int? TotalProduct { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public string? PaymentName { get; set; }
        public int? VoucherPercent { get; set; }
    }
}
