using ApartmentRental.Core.Models;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Specification;
using ApartmentRental.Reporisitory.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Reporisitory
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApartmentRentalContext _context;      
        public GenericRepository(ApartmentRentalContext context)
        {
            _context = context;
        }
        async Task IGenericRepository<T>.AddAsync(T entity)
        => await _context.Set<T>().AddAsync(entity);
        void IGenericRepository<T>.Delete(T entity)
        => _context.Set<T>().Remove(entity);
        void IGenericRepository<T>.Update(T entity)
          => _context.Set<T>().Update(entity);
        async Task<IReadOnlyList<T>> IGenericRepository<T>.GetAllAsync()
        =>await _context.Set<T>().ToListAsync();
        async Task<T> IGenericRepository<T>.GetByIdAsync(int id)
        => await _context.Set<T>().FindAsync(id);
        async Task<IReadOnlyList<T>> IGenericRepository<T>.GetAll(ISpecification<T> specification)
        => await ApplySpecification(specification).ToListAsync();
       async Task<T> IGenericRepository<T>.GetEntitybySpec(ISpecification<T> specification)
        => await ApplySpecification(specification).FirstOrDefaultAsync();
        async Task<int> IGenericRepository<T>.GetCount(ISpecification<T> specification)
        => await ApplySpecification(specification).CountAsync();
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
         => SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specification);
    }
}
