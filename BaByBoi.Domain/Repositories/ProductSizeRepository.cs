using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class ProductSizeRepository : GenericRepository<ProductSize>, IProductSizeRepository
    {
        public ProductSizeRepository(BaByBoiContext context) : base(context)
        {
        }
    }
}
