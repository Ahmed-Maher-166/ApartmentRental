using SmartRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Specification
{
    public class TenantByAppUserIdsSpecification : BaseSpecification<Tenant>
    {
        public TenantByAppUserIdsSpecification(int Params)
         : base(o => o.Id == Params)
        {
            Includes.Add(o => o.AppUser);
            Includes.Add(o => o.AppUser.Phones);

        }
    }
}
