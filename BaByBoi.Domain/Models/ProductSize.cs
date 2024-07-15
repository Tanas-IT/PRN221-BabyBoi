using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class ProductSize
    {
        public ProductSize()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public double? Price { get; set; }
        public int? SizeId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Size Size { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
