
using Microsoft.EntityFrameworkCore;
using SmartRental.Core.Models;
using SmartRental.Core.Repository;
using SmartRental.Core.Specification;
using SmartRental.Reporisitory;
using SmartRental.Reporisitory.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Reporisitory
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SmartRentalContext _context;      
        public GenericRepository(SmartRentalContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        => await _context.Set<T>().AddAsync(entity);
        public void Delete(T entity)
        => _context.Set<T>().Remove(entity);
        public void Update(T entity)
          => _context.Set<T>().Update(entity);
        public async Task<IReadOnlyList<T>> GetAllAsync()
        =>await _context.Set<T>().ToListAsync();
        public async Task<T> GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);
        public async Task<IReadOnlyList<T>> GetAll(ISpecification<T> specification)
        => await ApplySpecification(specification).ToListAsync();
        public async Task<T> GetEntitybySpec(ISpecification<T> specification)
        => await ApplySpecification(specification).FirstOrDefaultAsync();
        public async Task<int> GetCount(ISpecification<T> specification)
        => await ApplySpecification(specification).CountAsync();
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
         => SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specification);
        public async Task<T> GetFirstOrDefaultAsync(
                Expression<Func<T, bool>> predicate,
                Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }



       
    }
}
