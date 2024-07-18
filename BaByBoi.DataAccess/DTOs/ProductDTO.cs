using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }
        public string? Price { get; set; }
        public string? ImageUrl { get; set; }

        public int? Discount { get; set; }
    }
}
