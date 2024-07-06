using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class ProductImage
    {
        public int ImageId { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageName { get; set; }
        public string? CreateBy { get; set; }
        public int? ProductId { get; set; }
        public string? UpdateBy { get; set; }

        public virtual Product? Product { get; set; }
    }
}
