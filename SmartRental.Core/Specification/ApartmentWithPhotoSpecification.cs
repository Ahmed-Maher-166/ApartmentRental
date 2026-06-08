using SmartRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Specification
{
    public class ApartmentWithPhotoSpecification : BaseSpecification<Apartment>
    {
        public ApartmentWithPhotoSpecification(ApartmentSpecification Params) :
          base(p =>
                (string.IsNullOrEmpty(Params.StreetName) || p.StreetName.ToLower().Contains(Params.StreetName.ToLower())) &&
                (string.IsNullOrEmpty(Params.City) || p.City.ToLower().Contains(Params.City.ToLower()))
            )
        {
            Includes.Add(a => a.Photos);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                  
                    case "NameDesc":
                        AddByDSC(p => p.StreetName);
                        break;
                    default:
                        AddByASC(p => p.StreetName);
                        break;
                }
            }
            ApplyPagination(Params.PageSize, Params.PageSize * (Params.Index - 1));
        }
        public ApartmentWithPhotoSpecification(int id) : base(a => a.Id == id)
        {
            Includes.Add(a => a.Photos);
        }
    }
}
