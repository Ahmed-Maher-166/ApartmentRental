using ApartmentRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }


     public   Expression<Func<T, bool>>  Criteria { get   ; set   ; }

        public List<Expression<Func<T, object>>>  Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>>  OrederByASC { get   ; set   ; }
        public Expression<Func<T, object>>  OrederByDSC { get   ; set   ; }
        public bool  IsPagination { get   ; set   ; }
      public  int Take { get ; set; }
        public int  Skip { get   ; set   ; }
        public void AddByASC(Expression<Func<T, object>> OrederByASCEx)
        {
            OrederByASC = OrederByASCEx;
        }
        public void AddByDSC(Expression<Func<T, object>> OrederByDSCEx)
        {
            OrederByDSC = OrederByDSCEx;
        }

        public void ApplyPagination(int _Take, int _Skip)
        {
            IsPagination = true;
            Take = _Take;
            Skip = _Skip;
        }
    }

}


