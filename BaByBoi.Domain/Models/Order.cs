using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? OrderCode { get; set; }
        public int? UserId { get; set; }
        public string? Feedback { get; set; }
        public double? TotalPrice { get; set; }
        public int? TotalProduct { get; set; }
        public int? Rating { get; set; }
        public string? Address { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public int? PaymentId { get; set; }
        public int? VoucherId { get; set; }

        public virtual Payment? Payment { get; set; }
        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
