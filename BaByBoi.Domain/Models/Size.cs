using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class Size
    {
        public Size()
        {
            ProductSizes = new HashSet<ProductSize>();
        }

        public int SizeId { get; set; }
        public int? SizeCode { get; set; }
        public string? SizeName { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<ProductSize> ProductSizes { get; set; }
    }
}
