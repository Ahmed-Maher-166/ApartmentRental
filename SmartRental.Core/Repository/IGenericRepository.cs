
using SmartRental.Core.Models;
using SmartRental.Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region WithoutSpec
        public Task<T> GetByIdAsync(int id);
        public Task<IReadOnlyList<T>> GetAllAsync();
        public Task AddAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        #endregion
        #region WithSpec
        public Task<IReadOnlyList<T>> GetAll(ISpecification<T> specification);
        public Task<T> GetEntitybySpec(ISpecification<T> specification);
        public Task<int> GetCount(ISpecification<T> specification);
        #endregion
        Task<T> GetFirstOrDefaultAsync(
                  Expression<Func<T, bool>> predicate,
                  Func<IQueryable<T>, IQueryable<T>> include = null);
    }
}
