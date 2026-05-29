using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Models
{
    public class Photos : BaseEntity
    {
        public string PhotoUrl { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
    
    }
}
