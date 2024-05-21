using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly BaByBoiContext _context;

        public GenericRepository(BaByBoiContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            var a = _context.Set<T>().Find(id);
            return _context.Set<T>().Find(id);
        }

        public T GetById(string id)
        {
            var a = _context.Set<T>().Find(id);
            return _context.Set<T>().Find(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task<bool> RemovebyId(int id)
        {
            try
            {
                var entityID = GetById(id);

                if (entityID != null)
                {
                    _context.Remove<T>(entityID);
                    return await _context.SaveChangesAsync() > 0;
                }
                else
                {
                    Console.WriteLine("User not found for deletion");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting user: " + ex.Message);
                return false;
            }
        }

        public  async Task<bool> RemovebyId(string id)
        {
            try
            {
                var entityID = GetById(id);

                if (entityID != null)
                {
                    _context.Remove<T>(entityID);
                    return await _context.SaveChangesAsync() > 0;
                }
                else
                {
                    Console.WriteLine("User not found for deletion");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while deleting user: " + ex.Message);
                return false;
            }
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}
