using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class Cagetory
    {
        public Cagetory()
        {
            Products = new HashSet<Product>();
        }

        public int CagetoryId { get; set; }
        public string? CagetoryCode { get; set; }
        public string? CagetoryName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? CreateBy { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
