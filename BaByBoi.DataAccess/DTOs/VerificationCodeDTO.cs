using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.DTOs
{
    public class VerificationCodeDTO
    {
        public string Code { get; set; } = null!;
        public DateTime ExpirationTime { get; set; }
    }
}
