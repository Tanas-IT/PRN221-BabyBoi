using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            Orders = new HashSet<Order>();
        }

        public int VoucherId { get; set; }
        public string? VoucherCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Type { get; set; }
        public int? Percent { get; set; }
        public int? Value { get; set; }
        public int? TriggerValue { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
