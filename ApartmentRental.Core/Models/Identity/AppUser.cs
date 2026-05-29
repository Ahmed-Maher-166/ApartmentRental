using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string Photo { get; set; }
        public string NationalID { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Phones> Phones { get; set; } = new HashSet<Phones>();

    }
}
