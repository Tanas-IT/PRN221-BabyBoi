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
        T GetById(int id);
        T GetById(string id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        Task<bool> RemovebyId(int id);
        Task<bool> RemovebyId(string id);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
