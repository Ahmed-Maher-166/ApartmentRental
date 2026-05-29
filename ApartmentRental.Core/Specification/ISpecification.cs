using ApartmentRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; }
        public Expression<Func<T, object>> OrederByASC { get; set; }
        public Expression<Func<T, object>> OrederByDSC { get; set; }
        public bool IsPagination { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
