using SmartRental.Core.Models;
using SmartRental.Core.Repository;
using SmartRental.Reporisitory.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Reporisitory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmartRentalContext _dbContext;
        private Hashtable _repositories;
        public UnitOfWork(SmartRentalContext dbContext)
        {
            _dbContext = dbContext;

            _repositories = new Hashtable();
        }
       public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();

      public  async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>()
                   where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository =
                    new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}
