using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetUserByFullName(string fullName);
        User CheckLogin(string email, string password);
        Task<IEnumerable<User>> GetAll();
        Task<bool> ExistsAsync(int id);
    }
}
