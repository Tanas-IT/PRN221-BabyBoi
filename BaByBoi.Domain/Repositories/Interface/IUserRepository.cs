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
        Task<User> GetById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> CheckLogin(string email, string password);
        Task<User> ChangePasswordByEmail(string email, string newPassword);
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> SearchUser(string searchValue);
        Task<bool> ExistsAsync(int id);
    }
}
