using BaByBoi.DataAccess.Service.Interface;
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

        public User CheckLogin(string email, string password)
                => _unitOfWork.UserRepository.CheckLogin(email, password);

        public async Task<List<User>> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAll();
            return  users.ToList();
        }
    }
}
