using BaByBoi.DataAccess.Service.Interface;
using BaByBoi.Domain.Common.Enum;
using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.DataAccess.Service
{
    public class UserService : IUserService
    {
        public readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> CheckLogin(string email, string password)
                => await _unitOfWork.UserRepository.CheckLogin(email, Utils.PasswordHelper.ConvertToEncrypt(password));

        public Task<User> GetUserByEmail(string email)
                => _unitOfWork.UserRepository.GetUserByEmail(email);
        public async Task<bool> AddAsync(User user)
        {
            if (user.Password != null)
            {
                user.Password = Utils.PasswordHelper.ConvertToEncrypt(user.Password!);
            }
            return await _unitOfWork.UserRepository.AddAsync(user);
        }

        public async Task<User> ChangePasswordByEmail(string email, string newPassword)
                => await _unitOfWork.UserRepository.ChangePasswordByEmail(email, Utils.PasswordHelper.ConvertToEncrypt(newPassword));

        public async Task<List<User>> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            return users.ToList();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public async Task UpdateAsync(User user)
        {
            await _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await _unitOfWork.UserRepository.ExistsAsync(id);
        }
        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _unitOfWork.RoleRepository.GetAll();
        }
        public async Task CreateAsync(User user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
            {
                return false;
            }

            //// Update user status to '2' or perform deletion logic
            //user.Status = (int)StatusExist.Deleted; // Or set to 'Inactive', depending on your business logic
            //await _unitOfWork.UserRepository.UpdateAsync(user);

            await _unitOfWork.UserRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(); // Save changes in the Unit of Work

            return true;
        }

        public async Task<IEnumerable<User>> SearchUser(string searchValue)
        {
            return await _unitOfWork.UserRepository.SearchUser(searchValue);            
        }
    }
}
