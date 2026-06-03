using ApartmentRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Specification
{
    public class ApartmentWithFilteration : BaseSpecification<Apartment>
    {
        public ApartmentWithFilteration() { }
        public ApartmentWithFilteration(ApartmentSpecification Params) :
        base(p =>
                (string.IsNullOrEmpty(Params.StreetName) || p.StreetName.ToLower().Contains(Params.StreetName.ToLower())) &&
                (string.IsNullOrEmpty(Params.City) || p.City.ToLower().Contains(Params.City.ToLower()))
            )
        { }
    }
}
