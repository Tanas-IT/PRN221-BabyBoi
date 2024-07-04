using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
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
        public virtual async Task Add(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
           await _context.SaveChangesAsync();
        }
        public virtual async Task<bool> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync()> 0 ? true :false;
            return result;
        }

        public virtual async Task AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            var a = await _context.Set<T>().FindAsync(id);
            return a;
        }

        public virtual async Task<T> GetById(string id)
        {
            var a = await _context.Set<T>().FindAsync(id);
            return a;
        }

        public virtual async Task Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
           await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> RemovebyId(int id)
        {
            try
            {
                var entityID = await GetById(id);

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

        public virtual async Task<bool> RemovebyId(string id)
        {
            try
            {
                var entityID = await GetById(id);

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

        public virtual async Task RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
           var result = await _context.SaveChangesAsync() > 0 ? true : false;
            return result;
        }
    }
}
