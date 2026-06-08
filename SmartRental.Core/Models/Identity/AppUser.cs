using Microsoft.AspNetCore.Identity;
using SmartRental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRental.Core.Models.Identity
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Photo { get; set; }
        public string NationalID { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Phones> Phones { get; set; } = new HashSet<Phones>();
        public Owner Owner { get; set; }
        public Tenant Tenant { get; set; }
    }
}
