using ApartmentRental.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Models
{
    public class Phones : BaseEntity
    {
        public string PhoneNumber { get; set; }
       public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
