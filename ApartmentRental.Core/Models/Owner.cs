using ApartmentRental.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Models
{
    public class Owner : BaseEntity
    {
        public string AppUserId { get; set; } 
        public ICollection<Apartment> Apartments { get; set; } = new HashSet<Apartment>();
    }
}
