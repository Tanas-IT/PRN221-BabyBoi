using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public int? ProductSizeId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
        public virtual ProductSize? ProductNavigation { get; set; }
    }
}
