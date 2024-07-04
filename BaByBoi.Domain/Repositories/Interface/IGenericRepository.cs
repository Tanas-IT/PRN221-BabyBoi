using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<T> GetById(string id);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        Task Add(T entity);
        Task<bool> AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        Task Remove(T entity);
        Task<bool> RemovebyId(int id);
        Task<bool> RemovebyId(string id);
        Task RemoveRange(IEnumerable<T> entities);
        Task Update(T entity); 
        Task<bool> UpdateAsync(T entity);
    }
}
