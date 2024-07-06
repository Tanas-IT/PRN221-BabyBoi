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
        User CheckLogin(string email, string password);

    }
}
