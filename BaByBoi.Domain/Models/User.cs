using System;
using System.Collections.Generic;

namespace BaByBoi.Domain.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public string? FullName { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Status { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Gender { get; set; }
        public int? RoleId { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
