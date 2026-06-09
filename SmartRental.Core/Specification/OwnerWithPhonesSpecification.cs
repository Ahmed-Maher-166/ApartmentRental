using SmartRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Specification
{
    public class OwnersByAppUserIdsSpecification : BaseSpecification<Owner>
    {
        public OwnersByAppUserIdsSpecification(ApartmentSpecification Params)
               : base()
        {
            Includes.Add(o => o.AppUser);
            Includes.Add(o => o.AppUser.Phones);
            AddByASC(p => p.Id);
            ApplyPagination(Params.PageSize, Params.PageSize * (Params.Index - 1));

        }
        public OwnersByAppUserIdsSpecification(int Params)
          : base(o => o.Id == Params)
        {
            Includes.Add(o => o.AppUser);
            Includes.Add(o => o.AppUser.Phones);
            Includes.Add(o => o.Apartments);
        }
    }
}
