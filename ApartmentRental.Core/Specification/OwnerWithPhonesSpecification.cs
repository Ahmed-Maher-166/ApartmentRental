using ApartmentRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Specification
{
    public class OwnersByAppUserIdsSpecification : BaseSpecification<Owner>
    {
        public OwnersByAppUserIdsSpecification(List<string> appUserIds)
               : base(o => appUserIds.Contains(o.AppUserId))
        {
        }
    }
}
