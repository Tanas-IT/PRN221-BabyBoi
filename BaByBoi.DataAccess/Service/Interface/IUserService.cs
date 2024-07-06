using BaByBoi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service.Interface
{
    public interface IUserService
    {
        public Task<List<User>> GetAll();
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task<bool> UserExistsAsync(int id);
        Task<IEnumerable<Role>> GetAllRoles();
        Task CreateAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
