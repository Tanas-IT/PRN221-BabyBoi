using BaByBoi.DataAccess.Common.Enum;
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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Status == (int)StatusExist.Exist);
            if (user == null)
            {
                return null!;
            }

            return user;
        }

        public async Task<User> CheckLogin(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Password!.Equals(password) && u.Status == (int)StatusExist.Exist);
            if (user == null)
            {
                return null!;
            }

            return user;
        }

        public async Task<User> ChangePasswordByEmail(string email, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email) && u.Status == (int)StatusExist.Exist);
            if (user == null)
            {
                return null!;
            }
            user.Password = newPassword;
            var result = await _context.SaveChangesAsync() > 0;
            if (result)
            {
                return user;
            }

            return null!;
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(u => u.Role).Where(u => u.Status != (int)StatusExist.Deleted).ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchUser(string searchValue)
        {
            return await _context.Users.Include(u => u.Role).Where(u => u.Email.Contains(searchValue) && u.Status != (int)StatusExist.Deleted).OrderByDescending(u => u.UpdateDate).ToListAsync();
        }
        public override async Task<User> GetById(int id)
        {
            return await _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(e => e.UserId == id);
        }

        public async Task<int> GetTotalUserAsync()
        {
            var totalUser = await _context.Users
              .Where(o => o.Status == (int)StatusExist.Exist)
              .CountAsync();
            return totalUser;
        }
    }
}
