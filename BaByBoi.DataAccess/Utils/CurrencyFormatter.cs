using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Utils
{
    public static class CurrencyFormatter
    {
        public static string ToVietnameseCurrency(this double amount)
        {
            return amount.ToString("N0", new CultureInfo("vi-VN")) + " đ";
        }
    }
}
