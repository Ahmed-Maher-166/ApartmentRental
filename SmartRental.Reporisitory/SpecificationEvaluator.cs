using SmartRental.Core.Models;
using SmartRental.Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Reporisitory
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
      public static  IQueryable<T> GetQuery(
          IQueryable<T> Input, ISpecification<T> spec)
        {
            var query = Input; 
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrederByASC != null)
            {
                query = query.OrderBy(spec.OrederByASC);
            }
            if (spec.OrederByDSC != null)
            {
                query = query.OrderByDescending(spec.OrederByDSC);
            }
            if (spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            if (spec.Includes != null && spec.Includes.Any()) { 
                query = spec.Includes.Aggregate(
            query,
            (currentQuery, includeExpression) =>
                currentQuery.Include(includeExpression)
                );
            }
       
            return query;
        }
    }
}
