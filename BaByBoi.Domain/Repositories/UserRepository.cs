using BaByBoi.Domain.Common.Enum;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BaByBoiContext context) : base(context) { }

        public User GetUserByFullName(string fullName)
        {
          return _context.Users.FirstOrDefault(u => u.FullName == fullName)!;
        }

        public User CheckLogin(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(c => c.Email.Equals(email) && c.Password!.Equals(password) && c.Status == (int)StatusExist.Exist);
            if (user == null)
            {
                return null!;
            }

            return user;
        }
        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(e => e.UserId == id);
        }

    }
}
