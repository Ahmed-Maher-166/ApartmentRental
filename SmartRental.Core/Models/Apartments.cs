using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartRental.Core.Models
{
    public class Apartment : BaseEntity
    {
        public byte Rooms { get; set; }
        public byte Bathrooms { get; set; }
        public byte MaxTenants { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Gender Gender { get; set; } = Gender.Male;
        public Status AvailabilityStatus { get; set; } = Status.Available; 
        public string City { get; set; }
        public string StreetName { get; set; }
        public int BuildingNumber { get; set; }
        public byte ApartmentNumber { get; set; }
        public byte FloorNumber { get; set; }
         public int OwnerId { get; set; }
         public Owner Owner { get; set; }
        public ICollection<Photos> Photos { get; set; } = new HashSet<Photos>();
        public ICollection<RentalContract> RentalContracts { get; set; } = new HashSet<RentalContract>();

    }
}
