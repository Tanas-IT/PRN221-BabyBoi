using BaByBoi.Domain.Models;
using BaByBoi.Domain.Repositories.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaByBoi.Domain.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BaByBoiContext _myDbContext;
        private IDbContextTransaction _transaction;
        public IUserRepository UserRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }


        public UnitOfWork(BaByBoiContext myDbContext)
        {
            _myDbContext = myDbContext;
            UserRepository = new UserRepository(_myDbContext);

            ProductRepository = new ProductRepository(_myDbContext);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _myDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {

            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null!;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = true;


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _myDbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _myDbContext.SaveChangesAsync();
        }
    }
}
